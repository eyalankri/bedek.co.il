import Vue from 'vue'
import Vuex from 'vuex'
import { stat } from 'fs';
Vue.use(Vuex);


export const store = new Vuex.Store({
    state: {
        infoBarText: null,
        tokenHeader: null,
        apartmentComment:null,
        apiUrl: null,
        loggedInUser: null
        
    },

    mutations: {
        // SET:  this.$store.commit('setApartmentComment', data)            
        // GET:  this.$store.state.apartmentComment,
        setInfoBarText(state, text) {            
            state.infoBarText = text;
        },
        setApartmentComment(state, html) { 
            state.apartmentComment = html;
        },
        setLoggedInUser(state, email) { 
             
            state.loggedInUser = email;
        }

    },


    // use getters to manupilate the data
    //in the required component use in  computed():  saleproducts(){ return this.$store.getters.saleProducts }
    getters: {
        apiUrl() {             
           // return  "https://localhost:44349/api/"                        
            return ""; // moved to dev/prod.env.js
        },        
        
        getTokenHeader: state => {
            var token = localStorage.getItem("user-token");
            return {
                headers: {
                    Authorization: "Bearer " + token,
                    "Content-Type": "application/json"
                }

            }
        },
        getTokenHeaderFormData: state => {
            var token = localStorage.getItem("user-token");
            return {
                headers: {
                    Authorization: "Bearer " + token,
                    "Content-Type": "multipart/form-data"
                }

            }
        }

    }
})

