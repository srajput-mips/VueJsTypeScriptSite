
<template>
    
              <div id="app">
                <h3>Cats List</h3>
                <hr/>
                <div id="list">
                  <ul>
                    <li v-for="item in list" :key="item.gender">
                      {{ item.gender }}
                      <ul>
                        <li v-for="name in item.names" :key="name">
                          {{ name }}
                        </li>
                      </ul>
                    </li>
                  </ul>
                </div>
                <div class='col-md-4 col-md-offset-2 col-sm-6'>
                    <transition name='fade'>
                        <div v-if='showError' class='alert alert-danger' role='alert'>
                            <div v-if='validationErrors.length > 0'>
                            <ul  v-for='(err, index) in validationErrors' :key='index'>
                                <li>{{ err }}</li>
                            </ul>
                            </div>
                            {{ error }}
                        </div>
                    </transition>
                  
                    </div>

                    </div>
                       
                       
            
</template> 
 
<script lang='ts'>


    import Vue from 'vue'
    import Spinner from '@/components/ui/VsSpinner.vue' 
    import store from '@/store.ts'
    import axios from 'axios'    

    import {GET_CATS} from '@/vuex/action-types'
    import {CatsData} from '@/models/catsData'

    export default Vue.extend({

        async created () { 
           
              await this.fetchCats(); 
        },
        data () {

            return {
                list: [] as CatsData[], 
                validationErrors: [] as string[],
                error: '',     
                failed: false,
                isBusy : false, 
                loading:false

            }

        },      
        computed: { showError (): boolean { return this.error !== '' } },
        methods: {
          
            async fetchCats(){

              return await this.StateManagement();

            },
            async StateManagement(){
                //Perform action
                await this.PerformActions();
                //commit the mutation
                 await this.PerformCommit();

            },
            async PerformActions(){

              return this.$store.dispatch(GET_CATS)
                .then((list: CatsData[]) => {
                    this.list = list  
                });

            },
            async PerformCommit(){

             //commit the mutation

              if(this.list.length == 0){
                //error
                 //display network error
                  this.error = 'Error fetching results from the api';
                  this.$store.commit('SET_CATS', this.list);
              }else{
                //success 
                  this.$store.commit('SET_CATS', this.list);
              }             

            },
            components: {   Spinner }
        }
    })

</script>


<style>
 

* {
  box-sizing: border-box;
  font-family: 'Nunito', sans-serif;
}

html,
body {
  height: 100%;
  margin: 0;
  padding: 0;
  width: 100%;
}

div#app {
  width: 100%;
  height: 100%;
}

div#app div#login {
  align-items: center;
  background-color: #e2e2e5;
  display: flex;
  justify-content: center;
  width: 100%;
  height: 100%;
}

div#app div#login div#description {
  background-color: #ffffff;
  width: 280px;
  padding: 35px;
}

div#app div#login div#description h1,
div#app div#login div#description p {
  margin: 0;
}

div#app div#login div#description p {
  font-size: 0.8em;
  color: #95a5a6;
  margin-top: 10px;
}

div#app div#login div#form {
  background-color: #34495e;
  border-radius: 5px;
  box-shadow: 0px 0px 30px 0px #666;
  color: #ecf0f1;
  width: 260px;
  padding: 35px;
}

div#app div#login div#form label,
div#app div#login div#form input {
  outline: none;
  width: 100%;
}

div#app div#login div#form label {
  color: #95a5a6;
  font-size: 0.8em;
}

div#app div#login div#form input {
  background-color: transparent;
  border: none;
  color: #ecf0f1;
  font-size: 1em;
  margin-bottom: 20px;
}

div#app div#login div#form ::placeholder {
  color: #ecf0f1;
  opacity: 1;
}

div#app div#login div#form button {
  background-color: #ffffff;
  cursor: pointer;
  border: none;
  padding: 10px;
  transition: background-color 0.2s ease-in-out;
  width: 100%;
}

div#app div#login div#form button:hover {
  background-color: #eeeeee;
}

@media screen and (max-width: 600px) {
  div#app div#login {
    align-items: unset;
    background-color: unset;
    display: unset;
    justify-content: unset;
  }

  div#app div#login div#description {
    margin: 0 auto;
    max-width: 350px;
    width: 100%;
  }

  div#app div#login div#form {
    border-radius: unset;
    box-shadow: unset;
    width: 100%;
  }

  div#app div#login div#form form {
    margin: 0 auto;
    max-width: 360px;
    width: 100%;
  }
}
</style>