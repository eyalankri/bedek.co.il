<template>
  <div class="login container">
    <form  @submit.prevent="login" class="card-panel ltr">
      <h2 class="center deep-purple-text">Login</h2>
      <div class="field">
        <label for="email">Email:</label>
        <input type="email" name="email" v-model="email" autocomplete="email">
      </div>
      <div class="field">
        <label for="password">Password:</label>
        <input type="password" name="password" v-model="password" autocomplete="current-password">
      </div>

      <p class="red-text center" v-if="feedback">{{ feedback }}</p>

      <div class="field">
        <button class="btn deep-purple">Login</button>
      </div>

      <div class="progress" v-if="progressBar" style="margin-top:30px;">
        <div class="indeterminate"></div>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios"; // when it's located in node_modules we not need to specify the path

export default {
  name: "Login",
  data() {
    return {
      token: "",
      email: null,
      password: null,
      feedback: null,
      progressBar: null,
    };
  },
  mounted(){
    console.log(process.env.ROOT_API);
    
    this.$store.commit("setInfoBarText",null);  
    $('.navbar').hide();
  },
  methods: {
    login() {
      if (!this.email) {
        this.feedback = "Please enter your email.";
        return false;
      }
      if (!this.password) {
        this.feedback = "Please enter your password.";
        return false;
      }
      this.progressBar = true

      let userDetails = {
        Email: this.email,
        Password: this.password
      };

      console.log(process.env.ROOT_API + "Account/CreateToken")
      axios
        .post(process.env.ROOT_API + "Account/CreateToken", userDetails)
        .then(response => {
          $('.navbar').show();
          
          // store the token
          this.token = response.data.token;        
          localStorage.setItem("user-token", this.token);
          this.$store.commit('setLoggedInUser', this.email)    

            console.clear()
            console.log(response.data.token);
           $('.navbar').show();

           
          this.$router.push({
            name: "buildings"
          });
        })
        .catch(error => {
          this.feedback = "The details you provided are incorrect";
          localStorage.removeItem("user-token");
          this.progressBar = null;
        });
    }
  }
};
</script>

<style>
.login.container {
  max-width: 400px !important;
}
.login input {
  direction: ltr;
}
</style>
