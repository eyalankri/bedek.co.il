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
      <br /><br />
      <b>תאריך פתיחת פנייה</b>
      {{dateCreated}}
      <br />
      <b>תאריך עדכון אחרון</b>
      {{dateUpdated}}
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
      <div class="input-field col s12">
        <input id="serviceCallDescription" type="text" v-model="serviceCallDescription" />
        <label for="serviceCallDescription">מהות הפנייה</label>
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

    <div>
      <div class="row">
        <div class="input-field col s12 m6 l8">
          <input id="docDescription" type="text" v-model="docDescription" />
          <label for="docDescription">תיאור המסמך</label>
        </div>
        <div class="col s12 m6 l4">
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
              <input
                :checked="props.row.isChecked"
                type="checkbox"
                class="isChecked"
                :id="props.row.serviceInHandymanInBuildingId"
              />
              <span></span>
            </span>

            <span v-else v-html="props.formattedRow[props.column.field]"></span>
            <!-- show all other fields -->
          </template>
        </vue-good-table>
      </div>
    </div>
    <a
      v-if="isInsertButtonVisible"
      @click="insertServiceCall"
      class="waves-effect waves-light btn right"
    >שמור קריאת שירות</a>
    <p class="red-text right" v-if="feedback">{{ feedback }}</p>
    <div v-if="successfulySavedArea">
      <a class="waves-effect waves-light btn right">שלח לוואטסאפ</a>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";
import tiptap from "@/components/utilities/tiptap";
import moment from "moment";
import docs from "@/components/ServiceCall/docs";

export default {
  name: "serviceCall",
  components: { VueGoodTable, tiptap, docs },
   watch: {
    $route(to, from) {
     window.location.reload();
    }},
   
  data() {
    return {
      isUpdateProcess: false,
      progressBar: null,
      feedback: null,
      apartmentId: this.$route.params.apartmentId,
      serviceCallId: this.$route.params.serviceCallId,
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
      email: null,
      dateCreated: 'לא נפתח',
      dateUpdated: 'לא עודכן',
      warrantyPeriodInYears: null,
      insertButtonVisible: true,
      afterInsertAreaVisible: false,
      selectedLoopCounter: 1,
      isInsertButtonVisible: true,
      arrServiceInHandymanInBuildingId: [],
      successfulySavedArea: false,
      expandMoreDocs: true,
      docDescription: null,
      postedFile: null,
      isFileValid: null,
      rows: [],
      columns: [
        {
          field: "serviceInHandymanInBuildingId",
          type: "number",
          hidden: true
        },
        {
          label: "חוק המכר",
          field: "serviceName"
        },
        {
          label: "ש.אחריות",
          field: "warrantyPeriodInYears",
          type: "number"
        },
        {
          field: "isWarrantyExpired",
          type: "boolean",
          hidden: true
        },
        {
          label: "ימים מסוף אחריות",
          field: "warantyIcon",
          html: true
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
        })
        .catch(error => {
          console.log("loadApartmentInfo() :" + error);
        });
    },

  loadServiceCallInfo() {
      // also get the building info
      axios
        .get(
          process.env.ROOT_API +
            "ServiceInHandymanInBuildingInServiceCall/Get?serviceCallId=" +
          this.serviceCallId,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
         debugger;
         this.serviceCallDescription = res.data.description
         // this.email = res.data.user.email;
         // this.dateCreated = moment(res.data.dateOfEntrance).format("DD/MM/YYYY");
 $("label").addClass("active"); // will move the placeholder
        })
        .catch(error => {
          console.log("loadApartmentInfo() :" + error);
        });
    },



    selectionChanged(params) {
      // for some reason it runs twice - block the second one.
      if (this.selectedLoopCounter > 1) {
        this.selectedLoopCounter = 1;
        return false;
      }

      var isConfirmed = true;
      this.arrServiceInHandymanInBuildingId = [];

      var arrSelected = [];

      params.selectedRows.forEach(el => {
        if (el.isWarrantyExpired) {
          isConfirmed = confirm(
            "תוקף תקופת האחריות של חוק מכר זה הסתיימה. האם להמשיך?"
          );
        }
        if (!isConfirmed) {
          return false;
        }
        arrSelected.push(el.serviceInHandymanInBuildingId);
        this.arrServiceInHandymanInBuildingId.push(
          el.serviceInHandymanInBuildingId
        );
      });

      if (isConfirmed) {
        this.rows.forEach(el => {
          el.isChecked = false;
          if (arrSelected.includes(el.serviceInHandymanInBuildingId)) {
            el.isChecked = true;
          }
        });
      }

      this.selectedLoopCounter++;
    },
    
    insertServiceCall(params) {
      console.clear();

      this.feedback = null;
      if (this.arrServiceInHandymanInBuildingId.length == 0) {
        alert("יש לשייך חוק מכר");
        return false;
      }

      if (!this.isFileValid) return false;

      var formData = new FormData();
      formData.append("Description", this.serviceCallDescription);
      formData.append("ArrServiceInHandymanInBuildingId",this.arrServiceInHandymanInBuildingId);
      formData.append("ApartmentId", this.apartmentId);
      formData.append("DocDescription", this.docDescription);
      formData.append("PostedFile", this.postedFile);
      console.log(formData)

      axios
        .post(
          process.env.ROOT_API + "ServiceInHandymanInBuildingInServiceCall/Add",
          formData,
          this.$store.getters.getTokenHeaderFormData
        )
        .then(res => {
          console.log(res);
          console.log(res.data.serviceCallId);
          //this.isInsertButtonVisible = false;
          //this.successfulySavedArea = true;
          this.$router.push({
            name: "serviceCall",
            params: { apartmentId: this.apartmentId, serviceCallId:res.data.serviceCallId }
      });
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

          this.rows.forEach(el => {
            if (el.isChecked) {
              this.$set(this.rows[el.rowId], "vgtSelected", true);
            }
            if (el.isWarrantyExpired) {
              el.warantyIcon = `<i style='color:orange' class='material-icons'>warning</i> (${el.warrantyDaysElpased})`;
            }
          });
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
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
    }
  },

  mounted() {
    this.serviceCallId = this.$route.params.serviceCallId;
    
    this.loadApartmentInfo();
    this.loadServiceInHandymanInBuildingInServiceCall();
   
    
    if (this.serviceCallId != null) {
      this.isUpdateProcess = true;
    }

    this.$store.commit("setInfoBarText", "פתח קריאת שירות");
    if (this.isUpdateProcess) {             
        this.$store.commit("setInfoBarText", "עדכן קריאת שירות");
         this.loadServiceCallInfo();
    }
   
   

    $(".ProseMirror").text("");
  },
  
};
</script>