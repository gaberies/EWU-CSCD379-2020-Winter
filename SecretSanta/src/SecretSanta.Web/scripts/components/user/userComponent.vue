<template>
    <div>
        <button class="button" @click="createAuthor()">Create New</button>
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Email Address</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="user in users" :id="user.id">
                    <td>{{user.id}}</td>
                    <td>{{user.firstName}}</td>
                    <td>{{user.lastName}}</td>
                    <td>{{user.email}}</td>
                    <td>
                        <button class="button" @click='setAuthor(author)'>Edit</button>
                        <button class="button" @click='deleteAuthor(author)'>Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <author-details-component v-if="selectedAuthor != null"
                                  :author="selectedAuthor"
                                  @author-saved="refreshAuthors()"></author-details-component>
    </div>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import { User } from '../../secretsanta-client';
    import { App } from '../../app';
    import UserDetailsComponent from './userDetailsComponents.vue';

    @Component({
        components: {
            UserDetailsComponent
        }
    })

    export default class UsersComponent extends Vue {
        users: User[] = null;
        selectedUser: User = null;
        app: App.Main = new App.Main();

        async loadUsers() {
            this.users = await this.app.getAllUsers();
        }

        createUser() {
            this.selectedUser = <User>{};
        }

        async mounted() {
            await this.loadUsers();
        }

        setUser(user: User) {
            this.selectedUser = user;
        }

        async refreshUsers() {
            this.selectedUser = null;
            await this.loadUsers();
        }

        async deleteUser(user: User) {
            if (confirm(`Are you sure you want to delete ${user.firstName} ${user.lastName}`)) {
                await this.app.deleteUser(user);
            }
            await this.refreshUsers();
        }
    }
</script>