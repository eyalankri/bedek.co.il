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
        products: [ // example!
            { name: 'banana', price: 20 },
            { name: 'banana2', price: 30 },
        ],
    },
    mutations: {
        setInfoBarText(state, text) {            
            state.infoBarText = text;
        },
        setApartmentComment(state, html) { 
            state.apartmentComment = html;
        }

    },


    // use getters to manupilate the data
    //in the required component use in  computed():  saleproducts(){ return this.$store.getters.saleProducts }
    getters: {
        apiUrl() {             
           // return  "https://localhost:44349/api/"                        
            return ""; // moved to dev/prod.env.js
        },        
        saleProducts: state => {
            var saleProducts = state.products.map(prod => {
                return {
                    name: '**' + prod.name,
                    price: prod.price * 0.9
                }
            });
            return saleProducts;
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

