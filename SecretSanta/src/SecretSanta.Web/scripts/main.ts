import '../styles/site.scss';
import Vue from 'vue';
import UserComponent from './components/user/userComponent.vue';
import GiftComponent from './components/gift/giftComponent.vue';
import GroupComponent from './components/group/groupComponent.vue';

document.addEventListener("DOMContentLoaded", async () => {
    if (document.getElementById('userList')) {
        new Vue({
            render: h => h(UserComponent)
        }).$mount('#userList');
    }

    if (document.getElementById('giftList')) {
        new Vue({
            render: h => h(GiftComponent)
        }).$mount('#giftList');
    }

    if (document.getElementById('groupList')) {
        new Vue({
            render: h => h(GroupComponent)
        }).$mount('#groupList');
    }
});