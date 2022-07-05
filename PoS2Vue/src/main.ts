import { createApp, reactive } from 'vue'
import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(router)

app.use({
    install: (app) => {
        const state: any = reactive({
            isModalHidden: true,
        })

        app.provide("state", state)
    }
})

app.mount('#app')
