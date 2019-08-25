
<template>
  <div class="container">
    <div class="row">
      <b>שם הפרוייקט:</b>
      {{projectName}}
      <br />
      <b>כתובת הפרוייקט:</b>
      {{street}} {{buildingNumber}} {{city}}
      <br />
      <b>מספר הדירה:</b>
      {{apartmentNumber}}
      <br />
      <b>פרט הדייר:</b>
      {{firstName}} {{lastName}} {{phone1}} | {{phone2}}
      <br />
      <b>תאריך כניסה לדירה</b>
      {{dateOfEntrance }}
    </div>
    <div class="row">
      <div class="col s12 m12">
        סטטוס פנייה:
       
          <label>
            <input class="with-gap" name="group3" type="radio" checked />
            <span>חדשה</span>
          </label>
          <label>
            <input class="with-gap" name="group3" type="radio" checked />
            <span>סגורה</span>
          </label>
         
      </div>
    </div>
    <div class="row">
      <div class="input-field col s12 m12">
        <tiptap
          id="tiptap"
          style="padding:5px 0"
          ref="tiptap"
          :editorContent="this.serviceCallDescription"
        ></tiptap>
        <label for="tiptap-container">מהות הפנייה</label>
      </div>
    </div>
    <div class="row">
      <div class="col s12 m6">
        <div class="file-field input-field">
          <div class="btn">
            <span>בחר קובץ</span>
            <input type="file" id="apDoc" @change="onFileSelected" />
          </div>
          <div class="file-path-wrapper">
            <input class="file-path validate" type="text" placeholder="בחר מסמך" />
          </div>
        </div>
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
            styleClass="vgt-table"
          >
            <div slot="emptystate">אין נתונים בטבלה</div>
            
          <template slot="table-row" slot-scope="props" styleClass="vgt-table">
              <span v-if="props.column.field == 'delete'"><a href="javascript:;"><i @click="deleteApartmentDoc(props.row.apartmentDocId, props.row.docDescription)" class="material-icons">delete</i></a></span>
              <span v-else-if="props.column.field == 'download'" v-html="props.formattedRow[props.column.field]"></span>
              <span v-else-if="props.column.field == 'show'" v-html="props.formattedRow[props.column.field]"></span>     
              <span v-else-if="props.column.field == 'docDescription'">{{props.formattedRow[props.column.field]}}</span>
              <span v-else>
                {{props.formattedRow[props.column.field]}} <!--insert value-->
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
import tiptap from "@/components/utilities/tiptap";
import moment from "moment";

export default {
  name: "serviceCall",
  components: { VueGoodTable, tiptap },
  methods: {
    loadApartmentInfo() {
      // also get the building info
      axios
        .get(
          process.env.ROOT_API +
            "Apartment/Get?apartmentId=" +
            this.apartmentId,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.progressBar = false;
          console.log(res);
          // set the building info
          this.buildingId = res.data.buildingId;
          this.projectName = res.data.projectName;
          this.city = res.data.city;
          this.street = res.data.street;
          this.buildingNumber = res.data.buildingNumber;
          this.apartmentNumber = res.data.apartmentNumber;
          this.userId = res.data.user.userId;
          this.firstName = res.data.user.firstName;
          this.lastName = res.data.user.lastName;
          this.phone1 = res.data.user.phone1;
          this.phone2 = res.data.user.phone2;
          this.identityCardId = res.data.user.identityCardId;
          this.email = res.data.user.email;
          this.dateOfEntrance = moment(res.data.dateOfEntrance).format(
            "DD/MM/YYYY"
          );

          this.$store.commit(
            "setInfoBarText",
            `${projectName}: ${street} ${buildingNumber} ${city}.<b>11</b> דירה : ${res.data.apartmentNumber}`
          );
        })
        .catch(error => {
          console.log("loadApartmentInfo() :" + error);
        });
    },
    onFileSelected() {}
  },
  data() {
    return {
      progressBar: null,
      feedback: null,
      apartmentId: this.$route.params.apartmentId,
      serviceCallDescription: null,
      dateOfEntrance: null,
      buildingId: null,
      projectName: null,
      city: null,
      street: null,
      buildingNumber: null,
      apartmentNumber: null,
      userId: null,
      firstName: null,
      lastName: null,
      phone1: null,
      phone2: null,
      identityCardId: null,
      email: null
    };
  },
  mounted() {
    this.loadApartmentInfo();
    this.$store.commit("setInfoBarText", "קריאת שירות");
  }
};
</script>