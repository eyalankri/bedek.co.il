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
          <input class="with-gap" name="group3" type="radio" />
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
    <div class="row" v-if="this.serviceCallId">
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
          @on-selected-rows-change="selectionChanged"
          :columns="columns"
          :rows="rows"
          :search-options="{ enabled: true, placeholder: ' חפש בטבלה ', }"
          :rtl="true"
          :pagination-options="{ enabled: true, perPage: 50 , perPageDropdown: [100]}"
          styleClass="vgt-table condensed"
          :selectOptions="{
           enabled: true,
          selectOnCheckboxOnly: false, // only select when checkbox is clicked instead of the row
          selectionInfoClass: 'custom-class',
          selectionText: 'חוקי מכר נבחרו',
          clearSelectionText: '',
          disableSelectInfo: true, // disable the select info panel on top
}"
        >
          <div slot="emptystate">אין נתונים בטבלה</div>

          <template slot="table-row" slot-scope="props">
            <!--  if col name is 'isChecked' -->
            <span v-if="props.column.field == 'isChecked'">
              <input :checked="props.row.isChecked" type="checkbox" />
              <span></span>
            </span>
            <span v-else>{{props.formattedRow[props.column.field]}}</span>
          </template>
        </vue-good-table>
      </div>
    </div>
    <a @click="insertServiceCall" class="waves-effect waves-light btn right">שמור קריאת שירות</a>
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
  data() {
    return {
      progressBar: null,
      allowUpdate: false,
      feedback: null,
      apartmentId: this.$route.params.apartmentId,
      serviceCallDescription: null,
      dateOfEntrance: null,
      buildingId: null,
      serviceCallId: null,
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
      email: null,
      arrServiceInHandymanInBuildingId: [],
      rows: [],
      columns: [
        {
          label: "serviceInHandymanInBuildingId",  
          field: "serviceInHandymanInBuildingId",
          type: "number",
          
        },
        {
          label: "חוק המכר",
          field: "serviceName"
        },
        {
          label: "חברה",
          field: "company"
        },
        {
          label: "שם",
          field: "firstName"
        },
        {
          label: "משפחה",
          field: "lastName"
        },
        {
          label: "",
          field: "isChecked",
          html: true
        }
      ]
    };
  },
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
          console.log(res.data.projectName);
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
            `${this.projectName}: ${this.street} ${this.buildingNumber} ${this.city}. דירה : ${res.data.apartmentNumber}`
          );
        })
        .catch(error => {
          console.log("loadApartmentInfo() :" + error);
        });
    },
    selectionChanged(params) {
        
    var arr = [];
      params.selectedRows.forEach(function(el) {  
        arr.push(
          el.serviceInHandymanInBuildingId
        );        
      });
     
        this.arrServiceInHandymanInBuildingId = arr;
        console.log(this.arrServiceInHandymanInBuildingId)
    },
    insertServiceCall(params) {
        if (this.arrServiceInHandymanInBuildingId.length == 0) {
            alert("יש לשייך חוק מכר");
            return false;
        }    

      let serviceCall = {
          ApartmentId : this.apartmentId,
          //more prop in backend.
      }
      let apartmentId = this.apartmentId;
      console.log(params.selectedRows.length);
    
    

      axios
        .post(
          process.env.ROOT_API + "ServiceInHandymanInBuilding/Update",
          listDto,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          console.log(res);
        })
        .catch(error => {
          console.log(error);
        });
    },

    loadServiceInHandymanInBuildingInServiceCall() {
      this.rows = [];

      let url = `ServiceInHandymanInBuildingInServiceCall/List?apartmentId=${this.apartmentId}`;
      if (this.serviceCallId) {
        url = `ServiceInHandymanInBuildingInServiceCall/List?apartmentId=${this.apartmentId}&serviceCallId=${this.serviceCallId}`;
      }

      axios
        .get(
          process.env.ROOT_API + url,
          this.$store.getters.getTokenHeaderFormData
        )
        .then(response => {
          var cnt = 0;
          response.data.forEach(el => {
            el.rowId = cnt;
            cnt++;

            el.isChecked = el.isAssociated;
          });

          this.rows = response.data;

          this.allowUpdate = true;
          this.rows.forEach(el => {
            if (el.isChecked) {
              this.$set(this.rows[el.rowId], "vgtSelected", true); 
              this.allowUpdate = false; // if there are selected rows set allowUpdate=false // because in it will post to the server before any manulally selection
            }
          });
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
        });
    },
    onFileSelected() {}
  },

  mounted() {
    this.loadApartmentInfo();
    this.loadServiceInHandymanInBuildingInServiceCall();
    this.$store.commit("setInfoBarText", "קריאת שירות");
  }
};
</script>