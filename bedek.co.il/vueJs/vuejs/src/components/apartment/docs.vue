<template>
  <div class="container">
    <div class="row">
      <div class="input-field col s12 m6 l8">
        <input id="docDescription" type="text" v-model="docDescription">
        <label for="docDescription">תיאור המסמך</label>
      </div>
      <div class="col s12 m6 l4">
        <div class="file-field input-field">
          <div class="btn">
            <span>בחר קובץ</span>
            <input type="file" id="apDoc" @change="onFileSelected">
          </div>
          <div class="file-path-wrapper">
            <input class="file-path validate" type="text" placeholder="בחר מסמך">
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <p class="red-text right" v-if="feedback">{{ feedback }}</p>
      <div class="input-field col s12">
        <a @click="uploadDoc" class="waves-effect waves-light btn right">שמור מסמך</a>
      </div>
      <div class="progress" v-if="progressBar" style="margin-top:10px;">
        <div class="indeterminate"></div>
      </div>
    </div>
    <div class="row">
       

      <div id="vgt">
        
        <vue-good-table
            :columns="columns"
            :rows="rows"
            :rtl="true"
            :search-options="{ enabled: true,placeholder: ' חפש בטבלה ',}"
            :pagination-options="{ enabled: true, perPage: 10 , perPageDropdown: [50, 100]}"
            styleClass="vgt-table condensed"
          >
            <div slot="emptystate">אין נתונים בטבלה</div>
            
          <template slot="table-row" slot-scope="props">
              <span v-if="props.column.field == 'delete'"><a href="javascript:;"><i @click="deleteApartmentDoc(props.row.apartmentDocId, props.row.docDescription)" class="material-icons">delete</i></a></span>
              <span v-else-if="props.column.field == 'download'" v-html="props.formattedRow[props.column.field]"></span>
              <span v-else-if="props.column.field == 'show'" v-html="props.formattedRow[props.column.field]"></span>     
              <span v-else-if="props.column.field == 'docDescription'">{{props.formattedRow[props.column.field]}}</span>
              <span v-else>
                {{props.formattedRow[props.column.field]}}
              </span>              
          </template> 
          </vue-good-table>

      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";
import DataTable from "vue-materialize-datatable";
import moment from "moment";


export default {
  name: "apartmentDocs",
  components: { VueGoodTable },
  props: ["propApartmentId"],
  data() {
    return {
      feedback: null,
      progressBar: false,
      docDescription: null,
      isFileValid: null,
      apartmentId: this.propApartmentId,
      docDescription: null,
      postedFile: null,      
      buildingId: null,

      rows: [],
      columns: [         
        {
          label: "ת.העלאה",          
          field: "dateUploaded",
          
        },
        {
          label: "תיאור",
          field: "docDescription"
        },
        {
          label: "הורד",
          field: "download",
          html: true
        },
        {
          label: "הצג",
          field: "show",
            html: true
        },
        {
          label: "מחק",
          field: "delete",
            html: true
        }
      ]
    };
  },
  mounted() {
    this.listDocsInApartment();
  },
  methods: {
    deleteApartmentDoc(apartmentDocId, docDescription){
      var isConfirmed = confirm("למחוק את המסמך?\n\n תיאור: " + docDescription);
    
      if (!isConfirmed) return false;
      
       axios
        .get(
            process.env.ROOT_API + "ApartmentDocs/Delete?apartmentDocId=" +
            parseInt(apartmentDocId, 10),
            this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.listDocsInApartment();           
        })
        .catch(error => {
          console.log(error);
        });
    },
    onFileSelected() {
      this.feedback = null;
      this.isFileValid = null;
      this.postedFile = event.target.files[0];

      if (this.postedFile) {
        this.isFileValid = this.checkFileType(this.postedFile.type);
      } else {
        this.feedback = "יש לבחור קובץ";
        return false;
      }

      if (!this.isFileValid) {
        this.feedback = "לא ניתן להעלות קובץ מסוג זה.";
      }
    },
    uploadDoc() {
       this.feedback = null;
      if (!this.isFileValid) return false;

      var formData = new FormData();
      formData.append("ApartmentId", this.apartmentId);
      formData.append("DocDescription", this.docDescription);
      formData.append("PostedFile", this.postedFile);

      axios
        .post(
          process.env.ROOT_API + "ApartmentDocs/Upload",
          formData,
          this.$store.getters.getTokenHeaderFormData
        )
        .then(res => {
          console.log(res);
          this.feedback = "הקובץ נשמר בהצלחה.";
          this.listDocsInApartment();
        })
        .catch(error => {
          console.log(error);
        });
    },
    checkFileType(type) {
      let isFileValid = false;

      switch (type) {
        case "image/jpeg":
        case "image/png":
        case "text/plain":
        case "application/pdf":
        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": //excel
        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document": // word
        
          isFileValid = true;
          break;

        default:
          isFileValid = false;
      }
    
      return isFileValid;
    },
    listDocsInApartment() {
      
      axios
        .get(
            process.env.ROOT_API + "ApartmentDocs/List?apartmentId=" +
            this.apartmentId,
            this.$store.getters.getTokenHeader
        )
        .then(res => {
          res.data.forEach(res => {

             res.dateUploaded = moment(res.dateUploaded).format("DD/MM/YYYY");
             res.download =  `<a download href='Files/AppartmentsDocs/${res.buildingId}/${res.apartmentId}/${res.fileName}'><i class=" material-icons">file_download</i></a>`;
             res.show = `<a target="_blank" href='Files/AppartmentsDocs/${res.buildingId}/${res.apartmentId}/${res.fileName}'><i class=" material-icons">remove_red_eye</i></a>`;                           
             
          });

          //this.initializeDataTable();




          this.rows = res.data;


        })
        .catch(error => {
          console.log(error);
        });
    },  
    
  }
};

 

</script>

<style>
</style>
