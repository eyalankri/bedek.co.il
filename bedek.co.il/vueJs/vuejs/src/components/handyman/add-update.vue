<template>
  <div class="handyman">
    <div class="container">
      <form class="col s12">
        <div class="row">
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
            <input id="identityCardId" type="number" v-model="identityCardId">
            <label for="identityCardId">תעודת זהות</label>
          </div>
          <div class="input-field col s12 m6 6">
            <input id="email" type="email" v-model="email">
            <label for="email">אימייל</label>
          </div>
          <div class="input-field col s6 m6 6">
            <input id="company" type="text" v-model="company">
            <label for="company">חברה</label>
          </div>
        </div>
        <div class="row">
          <p class="red-text right" v-if="feedback">{{ feedback }}</p>
          <div class="input-field col s12">
            <a
              v-if="isInsert"
              @click="addHandynan"
              class="waves-effect waves-light btn right"
            >הוסף איש מקצוע</a>
            <a
              v-if="isUpdate"
              @click="updateHandyman"
              class="waves-effect waves-light btn right"
            >עדכן איש מקצוע</a>
             <a
                v-if="isUpdate"
                @click="$router.push({name:'handyman'})"
                class="waves-effect waves-light btn right"
              >חזרה לרשימה</a>
          </div>
          <div class="progress" v-if="progressBar" style="margin-top:30px;">
            <div class="indeterminate"></div>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";

export default {
  name: "addUpdateHandyman",
  props: ["propIsFromListHandyman"],
  
  data() {
    return {
      isUpdate: null,
      isInsert: null,

      feedback: null,
      handymanId: this.$route.params.handymanId,
      progressBar: null,
      userId: null,
      lastName: null,
      firstName: null,
      email: null,
      phone1: null,
      phone2: null,
      identityCardId: null,
      company: null
    };
  },
  mounted() {
    this.isInsert = this.propIsFromListHandyman;
    this.isUpdate = !this.propIsFromListHandyman;

    if (this.handymanId != null) {
      this.loadHandymanInfo();
    }
  },
  methods: {
    validateInputes() {
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

    addHandynan() {
      this.feedback = "";

      var isValid = this.validateInputes();
      if (!isValid) return false;

      this.progressBar = true;

      let data = {
        FirstName: this.firstName,
        LastName: this.lastName,
        Email: this.email,
        IdentityCardId: this.identityCardId,
        Phone1: this.phone1,
        Phone2: this.phone2,
        IsAcceptEmails: true,
        Company:this.company
      };
      axios
        .post(
          process.env.ROOT_API + "Handyman/Add",
          data,
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.feedback = "נשמר בהצלחה!";
          this.progressBar = false;

          this.firstName = null;
          this.lastName = null;
          this.email = null;
          this.identityCardId = null;
          this.phone1 = null;
          this.phone2 = null;
          this.company = null;

          this.$emit("loadApartmentList")
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },

    loadHandymanInfo() {
      // also get the building info
      axios
        .get(
          process.env.ROOT_API + "Handyman/Get?handymanId=" +
            this.handymanId,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.progressBar = false;

          this.$store.commit(
            "setInfoBarText",
            `שם נותן השירות: ${res.data.firstName} ${res.data.lastName}`
          );

          this.firstName = res.data.firstName;
          this.lastName = res.data.lastName;
          this.phone1 = res.data.phone1;
          this.phone2 = res.data.phone2;
          this.identityCardId = res.data.identityCardId;
          this.email = res.data.email;
          this.company = res.data.company;

          $("label").addClass("active"); // will move the placeholder
        })
        .catch(error => {
          console.log(error);
        });
    },

    updateHandyman() {
      var isValid = this.validateInputes();
      if (!isValid) return false;
      this.progressBar = true;

      let data = {
        UserId: this.handymanId,
        FirstName: this.firstName,
        LastName: this.lastName,
        Email: this.email,
        IdentityCardId: this.identityCardId,
        Phone1: this.phone1,
        Phone2: this.phone2,
        IsAcceptEmails: true,
        Company: this.company,
      };

      axios
        .post(
          process.env.ROOT_API + "Handyman/Update",
          JSON.stringify(data),
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.feedback = "עודכן בהצלחה!";
          this.progressBar = false;
          this.emitApartmentList();
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
    emitApartmentList: function() {
      this.$emit("refreshListFromEmit"); // call and refresh the list in the parent page
    }
  }
};
</script>

<style>
</style>
