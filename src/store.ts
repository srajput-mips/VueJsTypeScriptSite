import Vue from 'vue'
import Vuex, { StoreOptions } from 'vuex' 
import  {auth} from '@/vuex/modules/authenticaion'  
import  { cat} from '@/vuex/modules/cats'  
Vue.use(Vuex)
export interface RootState { token: true}

const store: StoreOptions<RootState> = {
    strict: true,
    state: { token:true},
    modules: { 
     auth ,
     cat
    }
}

export default new Vuex.Store<RootState>(store)