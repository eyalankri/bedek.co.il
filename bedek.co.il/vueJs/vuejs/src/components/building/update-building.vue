<template>
  <div class="add-building">
    <div class="container">
    
      <div class="row">
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
              <input id="number" name="buildingNumber" type="text" v-model="buildingNumber">
              <label for="buildingNumber">* מספר בניין</label>
            </div>
          </div>

          <div class="row">
            <p class="red-text right" v-if="feedback">{{ feedback }}</p>
            <div class="input-field col s12">
              <a @click="updateBuilding" class="waves-effect waves-light btn right">עדכן</a>
              &nbsp; <a @click="backToList" style="margin-right:5px" class="waves-effect waves-light btn right">חזרה לרשימה</a>
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

<script>
import axios from "axios";
export default {
  name: "updateBuilding",
  data() {
    return {
      feedback: null,
      street: null,
      buildingNumber: null,
      projectName: null,
      city: null,
      buildingId: null,
      progressBar: null
    };
  },
  mounted() {
    this.$store.commit("setInfoBarText", "עדכן בניין");
    this.progressBar = true;
    this.buildingId = this.$route.params.buildingId;
    this.loadBuilding();
  },
  methods: {
    loadBuilding() {
      
      let token = localStorage.getItem("user-token");
      var headers = {
        headers: {
          Authorization: "Bearer " + token
        }
      };

      axios
        .get(
          process.env.ROOT_API + "Building/Get?buildingId=" +
            this.buildingId,
          headers
        )
        .then(res => {
          $("label").addClass("active"); // will move the placeholder
          this.projectName = res.data.projectName;
          this.city = res.data.city;
          this.street = res.data.street;
          this.buildingNumber = res.data.buildingNumber;

          this.progressBar = false;
        })
        .catch(error => {
          console.log(error);
        });

     
    },
    updateBuilding() {
      this.feedback = "";

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
      
      this.progressBar = true;

      let data = {
        City: this.city,
        Street: this.street,
        BuildingNumber: this.buildingNumber,
        ProjectName: this.projectName,
        BuildingId: this.buildingId
      };

      let token = localStorage.getItem("user-token");

      var headers = {
        headers: {
          Authorization: "Bearer " + token,
          "Content-Type": "application/json"
        }
      };

      axios
        .post(process.env.ROOT_API + "Building/Update", data, headers)
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
    backToList(){
       this.$router.push({
            name: "buildings"
          });
    }
  }
};
</script>

<style>
</style>
