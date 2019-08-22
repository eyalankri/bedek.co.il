<template>
  <div class="container">
    <!-- add-building -->

    <button @click="show" type="button" class="btn btn-danger btn-circle btn-xl">
      <i style="color:white;font-size:1.5em;font-weight:bold" class="material-icons">add</i>
    </button>

    <modal name="modal">
      <i class="material-icons" @click="hide" style="cursor:pointer">close</i>
      <div class="modal-padding">
        <h3>הוסף בניין</h3>
        <form class="col s12">
          <div class="row">
            <div class="input-field col s12 m6">
              <input id="projectName" name="projectName" type="text" v-model="projectName" />
              <label for="projectName">שם הפרוייקט</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="city" name="city" type="text" v-model="city" />
              <label for="city">* עיר</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="street" name="street" type="text" v-model="street" />
              <label for="street">* רחוב</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="number" name="buildingNumber" type="text" v-model="buildingNumber" />
              <label for="buildingNumber">* מספר בניין</label>
            </div>
          </div>

          <div class="row">
            <p class="red-text right" v-if="feedback">{{ feedback }}</p>
            <div class="input-field col s12">
              <a @click="addBuilding" class="waves-effect waves-light btn right">הוסף בניין</a>
            </div>
            <div class="progress" v-if="progressBar" style="margin-top:30px;">
              <div class="indeterminate"></div>
            </div>
          </div>
        </form>
      </div>
    </modal>

    <!-- /add-building -->
    <!-- list-buildings -->
    <div class="list-buildings">
      <div class="progress" v-if="progressBar" style="margin-top:30px;">
        <div class="indeterminate"></div>
      </div>
      <div class="title-container hand" @click="toggleExpande('list')">
        <span class="title">בניינים קיימים</span>
        <i class="material-icons" v-if="expandMoreList">expand_more</i>
        <i class="material-icons" v-if="!expandMoreList">expand_less</i>
      </div>
      <div v-show="expandMoreList">
        <vue-good-table
          :columns="columns"
          :rows="rows"
          :rtl="true"
          :search-options="{ enabled: true,placeholder: ' חפש בטבלה ',}"
          :pagination-options="{ enabled: true, perPage: 50 , perPageDropdown: [100]}"
          styleClass="vgt-table condensed"
        >
          <div slot="emptystate">אין נתונים בטבלה</div>
        </vue-good-table>
      </div>
    </div>
    <!-- /list-buildings -->
  </div>
</template>




<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "buildings",
  components: { VueGoodTable },
  data() {
    return {
      progressBar: null,
      expandMoreAdd: false,
      expandMoreList: true,
      feedback: null,
      street: null,
      buildingNumber: null,
      projectName: null,
      city: null,
      buildingId: null,
      newBuilding: [],
      buildings: [],

      rows: [],
      columns: [
        {
          label: "Id",
          field: "buildingId",
          type: "number"
        },
        {
          label: "פרוייקט",
          field: "projectName"
        },
        {
          label: "עיר",
          field: "city"
        },
        {
          label: "רחוב",
          field: "street"
        },
        {
          label: "מספר",
          field: "buildingNumber",
          type: "numer"
        },
        {
          label: "עדכן",
          field: "updateBuilding",
          html: true
        },
        {
          label: "חוקי מכר",
          field: "serviceInHandymanInBuilding",
          html: true
        },
        {
          label: "דירות",
          field: "listApartment",
          html: true
        }
      ]
    };
  },
  mounted() {
    this.$store.commit("setInfoBarText", "בניינים");
    this.listBuildings();
  },
  methods: {
    show() {
      this.$modal.show("modal");
    },
    hide() {
      this.$modal.hide("modal");
    },
    toggleExpande(area) {
      if (area == "add") {
        this.expandMoreAdd = !this.expandMoreAdd;
      }
      if (area == "list") {
        this.expandMoreList = !this.expandMoreList;
      }
    },
    addBuilding() {
      this.feedback = "";
      this.progressBar = true;

      if (!this.city) {
        this.feedback = "יש לרשום עיר.";
        return false;
      }
      if (!this.street) {
        this.feedback = "יש לרשום כתובת.";
        return false;
      }
      if (!this.buildingNumber) {
        this.feedback = "יש לרשום את מספר הבניין.";
        return false;
      }

      let data = {
        City: this.city,
        Street: this.street,
        BuildingNumber: this.buildingNumber,
        ProjectName: this.projectName
      };

      axios
        .post(
          process.env.ROOT_API + "Building/Add",
          data,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.city = null;
          this.street = null;
          this.buildingNumber = null;
          this.projectName = null;

          this.feedback = "נשמר בהצלחה!";
          this.progressBar = null;
          var table = $("#buildings").DataTable();
          table.destroy(); // must destroy before calling again
          this.listBuildings();
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
    listBuildings() {

      this.progressBar = true;
      axios
        .get(
          process.env.ROOT_API + "Building/List",
          this.$store.getters.getTokenHeaderFormData
        )
        .then(response => {
          response.data.forEach(el => {
            let link = "";

            link = this.$router.resolve({
              name: "updateBuilding",
              params: { buildingId: el.buildingId }
            }).href;
            el.updateBuilding = `<a href='${link}'><i class="material-icons">edit</i></a>`;

            link = this.$router.resolve({
              name: "listApartment",
              params: { id: el.buildingId }
            }).href;
            el.listApartment = `<a href='${link}'><i class="material-icons">home</i></a>`;

            link = this.$router.resolve({
              name: "handymanInBuilding",
              params: { buildingId: el.buildingId }
            }).href;
            el.serviceInHandymanInBuilding = `<a href='${link}'><i class="material-icons">gavel</i></a>`;
          });

          this.rows = response.data;

          this.progressBar = null;
        })
        .catch(error => {
          console.log(error);
        });
    }
  }
};
</script>

 

<style>
div.dt-button-info {
  position: fixed;
  top: 50%;
  left: 50%;
  width: 400px;
  margin-top: -100px;
  margin-left: -200px;
  text-align: center;
  z-index: 21;
  color: rgba(0, 0, 0, 0.6);
}

@media screen and (max-width: 640px) {
  div.dt-buttons {
    float: none !important;
    text-align: center;
  }
}
</style>
