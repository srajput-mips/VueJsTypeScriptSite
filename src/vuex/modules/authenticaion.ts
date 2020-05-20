import {
    LOG_IN,LOG_IN_HARDCODED,
    LOG_OUT,
    INITIALIZE_AFTER_AUTH
} from '@/vuex/action-types'


import moment from 'moment'
import LocalData from '@/utils/local-data' 
import { User, AccessLevel } from '@/models/user' 
import { RootState } from '@/store'
import { Module, MutationTree, ActionTree, GetterTree } from 'vuex'
import { auth as authApi, authenticate as HardCodedAuth } from '@/api/fun-data'


export type Optional<T> = T | undefined

const DATA_KEY = 'auth'
const SET_USER = 'SET_USER'
const setErrorMessage = 'setErrorMessage'


export interface AuthState {
    error: string;
    isLoading: boolean;
    loadingMessage: string;
    user: Optional<User>;
    userName: string;
    userEmail: string;
    userId: string;
}

const state: AuthState = {
    error: '',
    isLoading: false,
    loadingMessage: '',
    user: undefined,
    userName: '',
    userEmail: '',
    userId: ''
}


const actions: ActionTree<AuthState, RootState> = {

    [LOG_IN] (store, {uid, pwd}) {
        const url = store.rootGetters.LoginApiUrl
        return authApi(uid, pwd, url)
        .then((loginData) => {
            if (loginData && loginData.success) {
                store.commit(SET_USER, loginData.user)
                loginData.timestamp = moment()
                LocalData.store(DATA_KEY, loginData)
                store.dispatch(INITIALIZE_AFTER_AUTH)
                return Promise.resolve()
            } else {
                return Promise.reject({message: loginData.message})
            }
        })
        .catch((err) => {
            if (err && err.message) {
                store.commit(setErrorMessage, err.message)
                return Promise.reject(err.message)
            } else {
                store.commit(setErrorMessage, 'Login failed')
                return Promise.reject('Login failed')
            }
        })
    }, 
    [LOG_IN_HARDCODED] (store, {uid, pwd}) {  
        const url = store.rootGetters.LoginApiUrl
     
        return HardCodedAuth(uid, pwd, url)
        .then((loginData) => {
            if (loginData && loginData.success) {  
                store.commit(SET_USER, loginData.user)
                loginData.timestamp = moment()
                LocalData.store(DATA_KEY, loginData)
                store.dispatch(INITIALIZE_AFTER_AUTH)
                return Promise.resolve()
            } else {  
                return Promise.reject({message: loginData.message})
            }
        })
        .catch((err) => {
            if (err && err.message) {
                store.commit(setErrorMessage, err.message)
                return Promise.reject(err.message)
            } else {
                store.commit(setErrorMessage, 'Login failed')
                return Promise.reject('Login failed')
            }
        })
    },
    [LOG_OUT] (store) {
        store.commit(SET_USER, undefined)
    }

}

const getters: GetterTree<AuthState, RootState> = {
    isAuthenticated: state => !!state.user,
    isAuthorized: state => !!state.user, 
    user: state => state.user!,
    loggedInName: state => state.user ? `${state.user.FirstName} ${state.user.LastName}` : ''
}

const mutations: MutationTree<AuthState> = {
    [setErrorMessage] (state, msg) { state.error = msg },
    [SET_USER] (state, user) {  
        state.user = user; 
        state.isLoading = false;
    }
}

export const auth: Module<AuthState, RootState> = {
    state,
    mutations,
    actions,
    getters
}
