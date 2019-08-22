<template>
  <div>
    <div class="container">
      <button @click="show" type="button" class="btn btn-danger btn-circle btn-xl"><i style="color:white;font-size:1.5em;font-weight:bold" class="material-icons">add</i></button>
      
      <modal name="add-apprtment">
        <i class="material-icons" @click="hide" style="cursor:pointer">close</i>
          <addUpdateApartment
            v-on:loadApartmentList="refreshListAppartments()"
            :propIsFromListApartment="true"
          />
        
      </modal>
    </div>

    <div class="container">
      <!-- list-buildings -->
      <div class="list-buildings">
        <div class="title-container hand" @click="toggleExpande('list')">
          <span class="title">דירות:</span>
          <i class="material-icons" v-if="expandMoreList">expand_more</i>
          <i class="material-icons" v-if="!expandMoreList">expand_less</i>
        </div>
        <div v-show="expandMoreList">
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
            </vue-good-table>
          </div>
        </div>
      </div>
      <!-- /list-buildings -->
    </div>
  </div>
</template>


<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";
import moment from "moment";
import addUpdateApartment from "@/components/apartment/add-update";
 

export default {
  name: "AppartmentList",
  components: {
    addUpdateApartment,
    VueGoodTable,
     
  },

  data() {
    return {
       
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
      IentidyCardId: null,

      rows: [],
      columns: [
        {
          label: "דירה",
          field: "apartmentNumber",
          type: "number"
        },
        {
          label: "ת.כניסה",
          field: "dateOfEntrance"
        },
        {
          label: "שם",
          field: "user.firstName"
        },
        {
          label: "משפחה",
          field: "user.lastName"
        },
        {
          label: "טלפון",
          field: "user.phone1"
        },
        {
          label: "הצג",
          field: "show",
          html: true
        }
      ]
    };
  },
  mounted() {
    
    this.loadBuildingInfo();
    this.listApartments();
  },
  methods: {
    show() {
      $('.ProseMirror').text('');      
      this.$modal.show("add-apprtment");
    },
    hide() {
      this.$modal.hide("add-apprtment");
    },
    loadBuildingInfo() {
      axios
        .get(
          process.env.ROOT_API +
            "building/Get?buildingId=" +
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
      axios
        .get(
          process.env.ROOT_API + "Apartment/List?buildingId=" + this.buildingId,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          console.log(res.data);
          res.data.forEach(res => {
            res.dateOfEntrance = moment(res.dateOfEntrance).format(
              "DD/MM/YYYY"
            );
            res.show = `<a href='/${
              this.$router.resolve({
                name: "showApartment",
                params: { id: res.apartmentId }
              }).href
            }'><i class="material-icons">perm_identity</i></a>`;
          });

          this.rows = res.data;
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
        });
    }
  }
};
</script>
 