<template>
<div>
   <div class="container title-container hand" @click="toggleExpande('add')">
      <span class="title">הוסף איש מקצוע</span>
      <i class="material-icons" v-if="expandMoreAdd">expand_more</i>
      <i class="material-icons" v-if="!expandMoreAdd">expand_less</i>
    </div>
    <div v-show="expandMoreAdd">
      <addUpdate 
        v-on:loadApartmentList="refreshListFromEmit()" 
        :propIsFromListHandyman="true"/>
    </div>
  <div class="container">   
    <!-- list-handyman -->
    <div class="list-handyman">
      <div class="title-container hand" @click="toggleExpande('list')">
        <span class="title">אנשי מקצוע רשומים</span>
        <i class="material-icons" v-if="expandMoreList">expand_more</i>
        <i class="material-icons" v-if="!expandMoreList">expand_less</i>
      </div>
      <div v-show="expandMoreList" class="material-table">
        <table id="handymans" class="mdl-data-table" width="100%"></table>
      </div>
    </div>
    <!-- /list-buildings -->
  </div>
</div>
</template>

<script>
import axios from "axios";
import moment from "moment";
import addUpdate from "@/components/handyman/add-update";

export default {
  name: "handyman",
  components: {
    name: "AppartmentList",
    addUpdate
  },

  data() {
    return {
      expandMoreAdd: true,
      expandMoreList: true,
      userId: null,
      isUpdate: null,
      isInsert: true,
      feedback: null,
      progressBar: null,
      userId: null,
      lastName: null,
      firstName: null,
      email: null,
      phone1: null,
      phone2: null,
      identityCardId: null,
      dataset: []
    };
  },
  mounted() {
    this.$store.commit("setInfoBarText", "אנשי מקצוע");
    var handymanId = this.$route.params.id;
    this.listHandyman();

    if (handymanId) {
      isUpdate = true;
    }
    if (!handymanId) {
      this.isInsert = true;
    }
  },

  methods: {
    toggleExpande(area) {
      if (area == "list") {
        this.expandMoreList = !this.expandMoreList;
      }
      if (area == "add") {
        this.expandMoreAdd = !this.expandMoreAdd;
      }
    },

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
        IsAcceptEmails: true
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
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
    updateHandyman() {},
    listHandyman() {
      axios
        .get(
          process.env.ROOT_API + "Handyman/List",
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          console.log(res);
          res.data.forEach(el => {
            this.dataset.push([
              el.company,
              el.firstName,
              el.lastName,
              el.phone1,
              el.phone2,
             
              `<a href='#/handyman/add-update/${el.userId}'><i class="material-icons">edit</i></a>`,
              `<a href='#/service-in-handyman/list/${el.userId}'><i class="material-icons">build</i></a>`

            ]);
          });

          this.initializeDataTable();
        })
        .catch(error => {
          console.log(error);
        });
    },
    initializeDataTable() {
      $("#handymans").DataTable({
        data: this.dataset,
        destroy: true,
        columns: [
           { title: "חברה" },
          { title: "שם" },
          { title: "משפחה" },
          { title: "טלפון" },
          { title: "טלפון" },         
          { title: "עדכן" },
          { title: "חוק מכר" },
        ]
      });
    },

    refreshListFromEmit() {       
      this.dataset = [];
      this.listHandyman();
    }
  }
};
</script>

<style>
</style>
