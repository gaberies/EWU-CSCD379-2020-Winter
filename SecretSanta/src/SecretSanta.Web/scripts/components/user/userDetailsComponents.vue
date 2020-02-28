<template>
    <div>
        <div class="field">
            <label class="label">First Name</label>
            <div class="control">
                <input class="input" type="text" v-model="clonedUser.firstName" />
            </div>
        </div>
        <div class="field">
            <label class="label">Last Name</label>
            <div class="control">
                <input class="input" type="text" v-model="clonedUser.lastName" />
            </div>
        </div>
        <div class="field is-grouped">
            <div class="control">
                <button id="submit" class="button is-primary" @click.once="saveUser">Submit</button>
            </div>
            <div class="control">
                <a class="button is-light" @click="cancelEdit">Cancel</a>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { Vue, Component, Prop, Emit } from 'vue-property-decorator';
    import { User } from '../../secretsanta-client';
    import { App } from '../../app';

    @Component
    export default class UserDetailsComponent extends Vue {
        @Prop()
        user: User;
        clonedUser: User = <User>{};
        app: App.Main;

        constructor() {
            super();
            this.app = new App.Main();
        }

        mounted() {
            let tempUser = { ...this.user };
            this.clonedUser = <User>tempUser;
        }

        @Emit('user-saved')
        async saveUser() {
            if (this.clonedUser.id > 0) {
                await this.app.putUser(this.clonedUser);
            }
            else {
                await this.app.postUser(this.clonedUser);
            }
        }

        @Emit('user-saved')
        cancelEdit() {

        }
    }
</script>