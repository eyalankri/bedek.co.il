<template>
  <div class="add-service">
    <div class="container">
      <div class="row">
        <form class="col s12">
          <div class="row">
            <div class="input-field col s12 m6">
              <input id="serviceName" name="serviceName" type="text" v-model="serviceName">
              <label for="serviceName">שם חוק המכר</label>
            </div>
            <div class="input-field col s12 m6">
              <input
                id="warrantyPeriodInYears"
                name="warrantyPeriodInYears"
                type="number"
                v-model="warrantyPeriodInYears"
              >
              <label for="warrantyPeriodInYears">* שנות אחריות</label>
            </div>
            <div class="input-field col s12 m6">
              <label>
                <input
                  type="checkbox"
                  id="isEnable"
                  name="isEnable"
                  v-model="isEnable"
                  checked="checked"
                >
                <span>* פעיל</span>
              </label>
            </div>
          </div>

          <div class="row">
            <p class="red-text right" v-if="feedback">{{ feedback }}</p>
            <div class="input-field col s12">
              <a
                v-if="isInsert"
                @click="addService"
                class="waves-effect waves-light btn right"
              >הוסף שירות</a>
              <a
                v-if="isUpdate"
                @click="updateService"
                class="waves-effect waves-light btn right"
              >עדכן שירות</a>
               <a
                v-if="isUpdate"
                @click="$router.push({name:'serviceList'})"
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
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";

export default {
  name: "addUpdateService",
  props: ["propIsFromListServices"],
  
  data() {
    return {
      isUpdate: null,
      isInsert: null,

      feedback: null,
      serviceId: this.$route.params.serviceId,
      warrantyPeriodInYears: null,
      serviceName: null,
      isEnable: null,
      progressBar: null
    };
  },
  mounted() {
    this.$store.commit("setInfoBarText", "הוסף שירות");
    
    this.isInsert = this.propIsFromListServices;
    this.isUpdate = !this.propIsFromListServices;  
    console.log(this.propIsFromListServices)  
    
     

    if (this.isUpdate == true) {
      this.loadServiceInfo();
    }
  },
  methods: {
    validateInputes() {
      this.feedback = "";

      if (!this.serviceName) {
        this.feedback = "יש לרשום את שם חוק המכר.";
        return false;
      }
      if (
        this.warrantyPeriodInYears <= 0 || !this.warrantyPeriodInYears) {
        this.feedback = "יש לרשום את שנות האחריות.";
        return false;
      }
      return true;
    },
    addService() {
      this.feedback = "";

      var isValid = this.validateInputes();
      if (!isValid) return false;

      this.progressBar = true;

      let data = {
        IsEnable: this.isEnable,
        WarrantyPeriodInMonths: (this.warrantyPeriodInYears * 12),
        ServiceName: this.serviceName,
        ServiceId: this.serviceId
      };

      axios
        .post(
          process.env.ROOT_API + "Service/Add",
          data,
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.feedback = "נשמר בהצלחה!";
          this.progressBar = false;

          this.warrantyPeriodInYears = null;
          this.serviceName = null;
          this.isEnable = null;
          this.emitServiceList(); 
        })
        .catch(error => {
          console.log(error);
          
        });
    },
    updateService() {
      var isValid = this.validateInputes();
      if (!isValid) return false;
      this.progressBar = true;

      let data = {
        serviceId: this.serviceId,
        IsEnable: this.isEnable,
        WarrantyPeriodInMonths: (this.warrantyPeriodInYears * 12),
        ServiceName: this.serviceName,
        ServiceId: this.serviceId        
      };

    debugger;
      axios
        .post(
          process.env.ROOT_API + "Service/Update",
          JSON.stringify(data),
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.feedback = "עודכן בהצלחה!";
          this.progressBar = false;
          this.emitServiceList();
          this.$emit("refreshListFromEmit"); 
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
    loadServiceInfo() {
      console.log("load");
      axios
        .get(
          process.env.ROOT_API + "Service/Get?serviceId=" +
            this.serviceId,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.progressBar = false;
         
         this.$store.commit(
            "setInfoBarText",
            `שם השירות: ${res.data.serviceName}`
          );

          this.serviceName = res.data.serviceName;
          this.isEnable = res.data.isEnable;
          this.warrantyPeriodInYears = (res.data.warrantyPeriodInMonths / 12);
          console.log(res.data)

          $("label").addClass("active"); // will move the placeholder
          
        })
        .catch(error => {
          console.log(error);
        });
    },
    
     emitServiceList: function() {
      this.$emit("loadApartmentList"); // call and refresh the List in the parent page
    },
    backToList() {
      this.$router.push({
        name: "serviceList"
      });
    }
  }
};
</script>

<style>
</style>
