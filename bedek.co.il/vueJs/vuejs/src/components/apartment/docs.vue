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
      <div class="material-table">
        <table id="apartmentDocs" class="mdl-data-table" width="100%"></table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import DataTable from "vue-materialize-datatable";
import moment from "moment";

export default {
  name: "apartmentDocs",
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
      dataset: null,
      buildingId: null
    };
  },
  mounted() {
    this.listDocsInApartment();
  },
  methods: {
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
      this.dataset = [];
      axios
        .get(
            process.env.ROOT_API + "ApartmentDocs/List?apartmentId=" +
            this.apartmentId,
            this.$store.getters.getTokenHeader
        )
        .then(res => {
          res.data.forEach(res => {
            this.dataset.push([
              res.apartmentDocId,            
               moment(res.dateUploaded).format("DD/MM/YYYY"),
              res.docDescription,                           
              `<a download href='Files/AppartmentsDocs/${res.buildingId}/${res.apartmentId}/${res.fileName}'><i class="large material-icons">file_download</i></a>`,
              `<a target="_blank" download href='Files/AppartmentsDocs/${res.buildingId}/${res.apartmentId}/${res.fileName}'><i class="large material-icons">remove_red_eye</i></a>`,

            ]);
          });

          this.initializeDataTable();
        })
        .catch(error => {
          console.log(error);
        });
    },
    initializeDataTable() {
      $("#apartmentDocs").DataTable({
        data: this.dataset,
        destroy: true,
        language: {
          processing: "מעבד...",
          lengthMenu: "הצג _MENU_ פריטים",
          zeroRecords: "לא קיימים מסמכים לדירה זו",
          emptyTable: "לא קיימים מסמכים לדירה זו",
          info: "_START_ עד _END_ מתוך _TOTAL_ רשומות",
          infoEmpty: "0 עד 0 מתוך 0 רשומות",
          infoFiltered: "(מסונן מסך _MAX_  רשומות)",
          infoPostFix: "",
          search: "חפש:",
          url: "",
          paginate: {
            first: "ראשון",
            previous: "קודם",
            next: "הבא",
            last: "אחרון"
          }
        },
        order: [0, "desc"],
        columns: [
          { title: "ID" },        
           { title: "ת.העלאה" },
          { title: "תיאור" },         
          { title: "הורד" },
          { title: "הצג" },
        ],
        columnDefs: [
          {
            targets: [1, 2, 3],
            className: "mdl-data-table__cell--non-numeric"
          },
          {
            // first col
            targets: [0],
            visible: false,
            searchable: false
          }
        ]
      });
    }
  }
};
</script>

<style>
</style>
