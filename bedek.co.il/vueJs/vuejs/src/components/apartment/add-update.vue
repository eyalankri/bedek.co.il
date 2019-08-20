<template>
  <div class="create-appartment">
    <div class="container">
      <div class="row">
        <form class="col s12">
          <div class="row">
            <div class="input-field col s3">
              <input id="apartmentNumber" type="number" v-model="apartmentNumber">
              <label for="apartmentNumber">מספר דירה</label>
            </div>
            <div class="input-field col s3">
              <input id="dateOfEntrance" type="text" class="datepicker">
              <label for="dateOfEntrance">תאריך כניסה</label>
            </div>
             <div class="input-field col s12">               
               <span style="color:#9e9e9e" for="tiptap">הערות לדירה</span>
            
            




            <!-- sendContent in the child / getContentFromEditor is belllow-->
            <tiptap id="tiptap" ref="tiptap" :editorContent="this.apartmentComment"></tiptap>






            </div>
          </div>
          <div class="row">
            <b>פרטי הדייר:</b>
            <input type="hidden" v-model="userId">
          </div>
          <div class="row">
            <div class="input-field col s6 m4 l3">
              <input id="firstName" type="text" v-model="firstName">
              <label for="firstName">שם פרטי</label>
            </div>
            <div class="input-field col s6 m4 l3">
              <input id="lastName" type="text" v-model="lastName">
              <label for="lastName">שם משפחה</label>
            </div>
            <div class="input-field col s6 m4 l2">
              <input id="phone1" type="tel" v-model="phone1">
              <label for="phone1">טלפון 1:</label>
            </div>
            <div class="input-field col s6 m4 l2">
              <input id="phone2" type="tel" v-model="phone2">
              <label for="phone2">טלפון 2:</label>
            </div>
            <div class="input-field col s6 m4 l2">
              <input id="identityCardId" type="text" v-model="identityCardId">
              <label for="identityCardId">תעודת זהות</label>
            </div>
            <div class="input-field col s12 m8 l6">
              <input id="email" type="email" v-model="email" class="ltr">
              <label for="email">אימייל</label>
            </div>
          </div>
          <div class="row">
            <p class="red-text right" v-if="feedback">{{ feedback }}</p>
            <div class="input-field col s12">
              <a
                v-if="this.isForAddingApartment"
                @click="addApartment"
                class="waves-effect waves-light btn right"
              >הוסף דירה</a>
              <a
                v-if="this.isForUpdatingApartment"
                @click="updateApartment"
                class="waves-effect waves-light btn right"
              >עדכן דירה</a>              
            </div>
            <div class="progress" v-if="progressBar" style="margin-top:30px;">
              <div class="indeterminate"></div>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<!-- this is the CHILD:-->
<script>
import axios from "axios";
import moment from "moment";

import tiptap from '@/components/utilities/tiptap';


export default {
  name: "AppartmentAdd",
  components: {
      tiptap,
  },
  props: ["propIsFromListApartment", "propApartmentId"],
  data() {
    return {
      apartmentId: null,
      feedback: null,
      progressBar: null,
      isForAddingApartment: null,
      isForUpdatingApartment: null,

      userId: null,
      lastName: null,
      firstName: null,
      email: null,
      phone1: null,
      phone2: null,
      identityCardId: null,
      apartmentNumber: null,
      dateOfEntrance: null,

      apartmentComment: null,
      residentComment:null
    };
  },

  mounted() {
    this.isForAddingApartment = this.propIsFromListApartment;
    this.isForUpdatingApartment = !this.propIsFromListApartment;

    this.initializeDatePicker();
    

    if (this.propApartmentId != null) {
      this.apartmentId = this.propApartmentId;
    } else {
      this.apartmentId = this.propApartmentId;
    }

    if (this.isForUpdatingApartment == true) {
      this.loadApartmentInfo();
    }
    
    
 
  },
  methods: {
    getContentFromEditor(html){
     this.apartmentComment = html;
    },
    validateInputes() {
      if (!this.apartmentNumber) {
        this.feedback = "יש לרשום את מספר הדירה.";
        return false;
      }

      if (!this.dateOfEntrance) {
        this.feedback = "יש לרשום את תאריך הכניסה.";
        return false;
      }
      if (!this.firstName) {
        this.feedback = "יש לרשום שם.";
        return false;
      }
      if (!this.lastName) {
        this.feedback = "יש לרשום שם משפחה.";
        return false;
      }
      if (!this.phone1) {
        this.feedback = "יש לרשום מספר טלפון או מספר נייד.";
        return false;
      }
      return true;
    },
    emitApartmentList: function() {
      this.$emit("loadApartmentList"); // call and refresh loadApartmentList in parent: list-apartment.vue
    },
    initializeDatePicker() {
      $(".datepicker").datepicker({
        selectMonths: true, // Creates a dropdown to control month
        selectYears: 15, // Creates a dropdown of 15 years to control year
        format: "dd/mm/yyyy"
      });
    },

    loadApartmentInfo() {
      // also get the building info
      axios
        .get(
       process.env.ROOT_API + "Apartment/Get?apartmentId=" +
            this.apartmentId,
             this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.progressBar = false;

          // set the building info
          this.buildingId = res.data.buildingId;
          let projectName = res.data.projectName;
          let city = res.data.city;
          let street = res.data.street;
          let buildingNumber = res.data.buildingNumber;

          this.$store.commit("setInfoBarText",`${projectName}: ${street} ${buildingNumber}, ${city}`);
          


          this.apartmentNumber = res.data.apartmentNumber;
          $(".datepicker").val(moment(res.data.dateOfEntrance).format("DD/MM/YYYY"));
          this.$store.commit('setApartmentComment', res.data.comment)
          
          
     
          this.userId = res.data.user.userId;
          this.firstName = res.data.user.firstName;
          this.lastName = res.data.user.lastName;
          this.phone1 = res.data.user.phone1;
          this.phone2 = res.data.user.phone2;

          this.identityCardId = res.data.user.identityCardId;
          this.email = res.data.user.email;

          $("label").addClass("active"); // will move the placeholder
          this.$emit('sendBuildingId', this.buildingId);
          
        })
        .catch(error => {
          console.log("loadApartmentInfo() :" + error);
        });
    },
    addApartment() {      
      this.dateOfEntrance = $(".datepicker").val();
      this.feedback = "";

      var isValid = this.validateInputes();
      if (!isValid) return false;
    
    this.progressBar = true;    
      let data = {
        BuildingId: this.$route.params.id,
        DateOfEntrance: moment(moment(this.dateOfEntrance, 'DD-MM-YYYY')).format('MM-DD-YYYY'),
        ApartmentNumber: this.apartmentNumber,
        Comment:   this.$store.state.apartmentComment,
        User: {
          FirstName: this.firstName,
          LastName: this.lastName,
          Email: this.email,
          IdentityCardId: this.identityCardId,
          Phone1: this.phone1,
          Phone2: this.phone2,
          IsAcceptEmails: true
        }
      }     
      axios
        .post(
          process.env.ROOT_API + "Apartment/Add",
          data,
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.feedback = "נשמר בהצלחה!";
          this.progressBar = false;

          this.buildingId = null;
          this.dateOfEntrance = null;
          $("#dateOfEntrance").val("");
          this.apartmentNumber = null;
          this.firstName = null;
          this.lastName = null;
          this.email = null;
          this.identityCardId = null;
          this.phone1 = null;
          this.phone2 = null;
        $('.ProseMirror').html('') // clear the tiptap editor
          


          this.emitApartmentList();
        })
        .catch(error => {
          console.log("loadApartmentInfo() :" + error)
        });
    },
    updateApartment() {
      let dateString = $("#dateOfEntrance").val();
      let momentObj = moment(dateString, "DD/MM/YYYY");
      this.dateOfEntrance = momentObj.format("MM/DD/YYYY");

      var isValid = this.validateInputes();
      if (!isValid) return false;
      this.progressBar = true;

      let data = {
        ApartmentId: this.$route.params.id,
        DateOfEntrance: this.dateOfEntrance,
        ApartmentNumber: this.apartmentNumber,
        Comment:   this.$store.state.apartmentComment,
        User: {
          UserId: this.userId,
          FirstName: this.firstName,
          LastName: this.lastName,
          Email: this.email,
          IdentityCardId: this.identityCardId,
          Phone1: this.phone1,
          Phone2: this.phone2,
          IsAcceptEmails: true
        }
      };
      console.log(data);
      axios
        .post(
          process.env.ROOT_API + "apartment/update",
          JSON.stringify(data),
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.feedback = "עודכן בהצלחה!";
          this.progressBar = false;
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
   
  }
};
</script>

<style>
</style>
