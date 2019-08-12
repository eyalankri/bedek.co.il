<template>
  <div>
    <div class="container title-container hand" @click="toggleExpande('add')">
      <span class="title">הוסף דירה חדשה</span>
      <i class="material-icons" v-if="expandMoreAdd">expand_more</i>
      <i class="material-icons" v-if="!expandMoreAdd">expand_less</i>
    </div>
    <div v-show="expandMoreAdd">
      <addUpdateApartment
        v-on:loadApartmentList="refreshListAppartments()"
        :propIsFromListApartment="true"
      />
    </div>
    <div class="container">
      <!-- list-buildings -->
      <div class="list-buildings">
        <div class="title-container hand" @click="toggleExpande('list')">
          <span class="title">דירות:</span>
          <i class="material-icons" v-if="expandMoreList">expand_more</i>
          <i class="material-icons" v-if="!expandMoreList">expand_less</i>
        </div>
        <div v-show="expandMoreList" class="material-table">
          <table id="apartment" class="mdl-data-table" width="100%"></table>
        </div>
      </div>
      <!-- /list-buildings -->
    </div>
  </div>
</template>


<script>
import axios from "axios";
import DataTable from "vue-materialize-datatable";
import moment from "moment";
import addUpdateApartment from "@/components/apartment/add-update";

export default {
  components: {
    name: "AppartmentList",
    addUpdateApartment
  },

  data() {
    return {
      dataset: [],
      expandMoreAdd: false,
      expandMoreList: true,
      feedback: null,
      buildingId: this.$route.params.id,
      ApartmentNumber: null,
      DateOfEntrance: null,
      FirstName: null,
      LastName: null,
      Email: null,
      Phone1: null,
      Phone2: null,
      IentidyCardId: null
    };
  },
  mounted() {
    this.loadBuildingInfo();
    this.listApartments();
  },
  methods: {
    loadBuildingInfo() {
      
      axios
        .get(
          process.env.ROOT_API + "building/Get?buildingId=" +
          this.$route.params.id,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.progressBar = false;

          let projectName = res.data.projectName;
          let city = res.data.city;
          let street = res.data.street;
          let buildingNumber = res.data.buildingNumber;

          this.$store.commit(
            "setInfoBarText",
            `${projectName}: ${street} ${buildingNumber}, ${city}`
          );
        });
    },
    refreshListAppartments() {
      // this call from $emit

      this.listApartments();
    },
    toggleExpande(area) {
      if (area == "add") {
        this.expandMoreAdd = !this.expandMoreAdd;
      }
      if (area == "list") {
        this.expandMoreList = !this.expandMoreList;
      }
    },
    listApartments() {
      this.dataset = [];
 
      axios
        .get(
          process.env.ROOT_API + "Apartment/List?buildingId=" +
            this.buildingId,
            this.$store.getters.getTokenHeader
        )
        .then(response => {
          response.data.forEach(el => {
            this.dataset.push([
              el.apartmentId,
              el.apartmentNumber,
              moment(el.dateOfEntrance).format("MM/DD/YYYY"),
              el.user.firstName,
              el.user.lastName,
              el.user.phone1,
              `<a href='/${
                this.$router.resolve({
                  name: "showApartment",
                  params: { id: el.apartmentId }
                }).href
              }'><i class="material-icons">perm_identity</i></a>`
            ]);
          });

          this.initializeDataTable();
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
        });
    },
    initializeDataTable() {
      // create & bind the data.
      $("#apartment").DataTable({
        data: this.dataset,
         language: {
          processing: "מעבד...",
          lengthMenu: "הצג _MENU_ פריטים",
          zeroRecords: "לא קיימות דירות לבניין זה",
          emptyTable: "לא קיימות דירות לבניין זה",
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
        destroy: true,
        order: [0, "desc"],
        columns: [
          { title: "ID" },
          { title: "דירה" },
          { title: "ת.כניסה" },
          { title: "שם" },
          { title: "משפחה" },
          { title: "טלפון" },
          { title: "הצג" }
        ],
        columnDefs: [
          {
            targets: [2, 3, 4, 5, 6],
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

      $(".mdl-cell--6-col:first").attr(
        "class",
        "mdl-cell--12-col-phone mdl-cell--2-col"
      );
      $(".mdl-cell--6-col").attr(
        "class",
        "mdl-cell--12-col-phone mdl-cell--10-col"
      );
    }
  }
};
</script>

<style>
</style>
