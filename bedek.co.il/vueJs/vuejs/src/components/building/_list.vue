<template>
  <div class="container">
    <!-- add-building -->
    <div class="add-building">
      <div class="title-container hand" @click="toggleExpande('add')">
        <span class="title">22222הוסף בניין</span>
        <i class="material-icons" v-if="expandMoreAdd">expand_more</i>
        <i class="material-icons" v-if="!expandMoreAdd">expand_less</i>
      </div>
      <div v-show="expandMoreAdd">
        <form class="col s12">
          <div class="row">
            <div class="input-field col s12 m6">
              <input id="projectName" name="projectName" type="text" v-model="projectName">
              <label for="projectName">שם הפרוייקט</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="city" name="city" type="text" v-model="city">
              <label for="city">* עיר</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="street" name="street" type="text" v-model="street">
              <label for="street">* רחוב</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="number" name="number" type="text" v-model="number">
              <label for="buildingNumber">* מספר בניין</label>
            </div>
          </div>

          <div class="row">
            <p class="red-text right" v-if="feedback">{{ feedback }}</p>
            <div class="input-field col s12">
              <a @click="addBuilding" class="waves-effect waves-light btn right">הוסף בניין</a>
            </div>
          </div>
        </form>
      </div>
      <div v-if="newBuilding.length > 0">
        <h5 class="right">בניינים שהוספת עכשיו:</h5>
        <table class="highlight">
          <thead>
            <tr class="teal lighten-4">
              <th>שם הפרוייקט</th>
              <th>עיר</th>
              <th>רחוב</th>
              <th>מספר בניין</th>
            </tr>
            <tr v-for="(b,i) in newBuilding.slice().reverse()" :key="i">
              <td>{{b.projectName}}</td>
              <td>{{b.city}}</td>
              <td>{{b.street}}</td>
              <td>{{b.number}}</td>
            </tr>
          </thead>
        </table>
      </div>
    </div>
    <!-- /add-building -->
    <!-- list-buildings -->
    <div class="list-buildings">
      <div class="title-container hand" @click="toggleExpande('list')">
        <span class="title">בניינים קיימים</span>
        <i class="material-icons" v-if="expandMoreList">expand_more</i>
        <i class="material-icons" v-if="!expandMoreList">expand_less</i>
      </div>
      <div v-show="expandMoreList" class="material-table">
        <table id="buildings" class="mdl-data-table" width="100%"></table>
      </div>
    </div>
    <!-- /list-buildings -->
  </div>
</template>




<script>
import axios from "axios";
import DataTable from "vue-materialize-datatable";

export default {
  name: "BuildingList",
  components: {
    datatable: DataTable
  },
  data() {
    return {
      expandMoreAdd: true,
      expandMoreList: true,
      feedback: null,
      street: null,
      number: null,
      projectName: null,
      city: null,
      newBuilding: [],
      buildings: [],
      dataset: []
    };
  },
  methods: {
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

      if (!this.city) {
        this.feedback = "יש לרשום עיר.";
        return false;
      }
      if (!this.street) {
        this.feedback = "יש לרשום כתובת.";
        return false;
      }
      if (!this.number) {
        this.feedback = "יש לרשום את מספר הבניין.";
        return false;
      }

      let data = {
        City: this.city,
        Street: this.street,
        Number: this.number,
        ProjectName: this.projectName
      };

      let token = localStorage.getItem("user-token");

      var headers = {
        headers: {
          Authorization: "Bearer " + token,
          "Content-Type": "application/json"
        }
      };

      axios
        .post(process.env.ROOT_API + "Building/Add", data, headers)
        .then(response => {
          this.newBuilding.push({
            // show the last added building.
            city: this.city,
            street: this.street,
            number: this.number,
            projectName: this.projectName
          });

          var addUpdateApartment = this.$router.resolve({
            name: "addUpdateApartment"
          }).href;
          var updateBuilding = this.$router.resolve({
            name: "updateBuilding",
            params: { buildingId: this.buildingId }
          }).href;


          this.dataset.push([
            this.city,
            this.street,
            this.number,
            this.projectNamem,
            `<a href='${addUpdateApartment}'>הוסף דירה</a>`,
            `<a href='${updateBuilding}'>עדכן בניין</a>`
          ]);

          this.city = null;
          this.street = null;
          this.number = null;
          this.projectName = null;

          this.initializeDataTable();
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
    listBuildings() {
      let token = localStorage.getItem("user-token");
      var headers = {
        headers: {
          Authorization: "Bearer " + token
        }
      };

      axios
        .get(process.env.ROOT_API + "Building/List", headers)
        .then(response => {
          response.data.forEach(el => {
            this.dataset.push([
              el.city,
              el.street,
              el.number,
              el.projectName,
              `<a href='#/add-appartment/${el.buildingId}'>הוסף דירה</a>`,
              `<a href='#/update-building/${el.buildingId}'>עדכן בניין</a>`
            ]);
          });

          this.initializeDataTable();
        })
        .catch(error => {
          console.log(error);
        });
    },
    initializeDataTable() {
      $("#buildings").DataTable({
        data: this.dataset,
        columns: [
          { title: "פרוייקט" },
          { title: "עיר" },
          { title: "רחוב" },
          { title: "מספר" },
          { title: "עדכן" },
          { title: "דירות" }
        ],
        columnDefs: [
          {
            targets: [0, 1, 2],
            className: "mdl-data-table__cell--non-numeric"
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
  },

  mounted() {
    this.listBuildings();
  }
};
</script>
