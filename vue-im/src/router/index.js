import Vue from 'vue'
import Router from 'vue-router'
import imServer from '@/components/imServer/imServer'
import imClient from '@/components/imClient/imClient'
import signalServer from '@/signal/server'
import signalClient from '@/signal/client'

Vue.use(Router)

export default new Router({
    routes: [
        { path: '/', redirect: 'imServer' },
        { path: '/imServer', name: 'imServer', component: imServer },
        { path: '/imClient', name: 'imClient', component: imClient },
        { path: '/signalServer', name: 'signalServer', component: signalServer },
        { path: '/signalClient', name: 'signalClient', component: signalClient },
    ]
})