import * as action from '@/vuex/action-types'
import { RootState } from '@/store'  
import { Module, MutationTree, ActionTree } from 'vuex' 
import {  getCats as GetCats } from '@/api/back-endApi'
 
export interface CatsState {

    error: string;
    isLoading: boolean;
}


const state: CatsState = { 
    error: '',
    isLoading: false  
}

const SET_CATS = 'SET_CATS'
 

const actions: ActionTree<CatsState,RootState> = {

    [action.GET_CATS] (store) { 

        const result = GetCats();
 
        store.commit(SET_CATS, result);

        return result;
    }

}

const mutations: MutationTree<CatsState> = { 
    [SET_CATS] (state, catData) {  
        state.isLoading = false;   
        state.error = '';
    }
}



export const cat: Module<CatsState, RootState> = {
    state, 
    mutations,
    actions
}
