<template>  
  <div class='application-container'> 

    <navigation-bar /> 

        <div style="margin-top:70px" class='container'>
            <div class='row'>
                <div class='col-sm-2' v-if='isAuthenticated'>
                        <router-link to="/Welcome">Welcome</router-link>
                        <hr/>
                        <router-link to="/About">About</router-link>
                         <hr/>
                        <router-link to="/Home">Home</router-link>
                        <hr/> 
                         <a href="javascript:void(0)"   v-on:click="logout" >Logout</a>

                </div>
                <div :class='containerClass'>
                    <spinner ref='spinner' size='md' :text='loadingMessage'></spinner>
                    <transition :name='transitionName' mode='out-in'>
                        <router-view class='child-view' :key='$route.fullPath'></router-view>
                    </transition>
                </div>
            </div>
        </div>
    </div>
</template>



<script lang='ts'>
    import Vue from 'vue'
    import {Route} from 'vue-router'
    import components from '@/components/app/components'  
    import { LOG_OUT } from '@/vuex/action-types'
 

    export default Vue.extend({
        data () {
            return {
                transitionName: 'fade',
                spinnerText: 'Loading'
            }
        },
        computed: { 
            isAuthenticated (): boolean { return this.$store.getters.isAuthenticated || false }, 
            containerClass (): string {return 'col-sm-10' }, 
               isLoading (): boolean { return true },
        },
        methods:{

            logout () {

                 this.$store.dispatch(LOG_OUT);
                 this.$router.push('/Login');
               
                
            },
        },
        watch: {
            'isLoading' (val: boolean) {
                (this.$refs.spinner as any)[val ? 'show' : 'hide']()
            },
            '$route' (to: Route, from: Route) {
                const toStage = to.meta && to.meta.stage ? to.meta.stage : 0
                const fromStage = from.meta && from.meta.stage ? from.meta.stage : 0
                if (toStage === 0) {
                    this.transitionName = 'fade'
                   // this.$store.dispatch(DEACTIVATE_TRANSACTION)
                    return
                }

                if (toStage < fromStage) {
                    this.transitionName = 'slide-right'
                } else {
                    this.transitionName = 'slide-left'
                }
            }
        },
        components
    })
</script>



<style lang='scss'>
    $icon-font-path: "../node_modules/bootstrap-sass/assets/fonts/bootstrap/";
    @import '../node_modules/bootstrap-sass/assets/stylesheets/_bootstrap';
 
</style>

<style>
    html {
        overflow-x: hidden;
        margin-right: calc(-1 * (100vw - 100%));
        padding-top: 70px;
    }
    .application-container .alert.top {
        z-index: 9999;
    }
    .fade-enter-active, .fade-leave-active {
        transition: opacity .5s ease;
    }
    .fade-enter, .fade-leave-active {
        opacity: 0
    }
    .child-view {
        transition: all .3s cubic-bezier(.55,0,.1,1);
    }
    .slide-left-enter, .slide-right-leave-active {
        opacity: 0;
        -webkit-transform: translate(300px, 0);
        transform: translate(300px, 0);
    }
    .slide-left-leave-active, .slide-right-enter {
        opacity: 0;
        -webkit-transform: translate(-300px, 0);
        transform: translate(-300px, 0);
    }
    ul.dropdown-menu li a.sq {
        float: left;
        width: 90%;
    }
    pre {
        white-space: pre-wrap;
        white-space: -moz-pre-wrap;
        white-space: -pre-wrap;
        white-space: -o-pre-wrap;
        word-wrap: break-word;
    }
    a {
        cursor: pointer
    }
</style>
