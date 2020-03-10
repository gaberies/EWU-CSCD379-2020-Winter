<template>
    <div class="modal is-active">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="field">
                <label class="label has-text-white">First Name</label>
                <div class="control">
                    <input id="user-title-firstName" class="input" type="text" v-model="clonedUser.firstName" />
                </div>
            </div>
            <div class="field">
                <label class="label has-text-white">Last Name</label>
                <div class="control">
                    <input id="user-title-lastName" class="input" type="text" v-model="clonedUser.lastName" />
                </div>
            </div>
            <div class="field is-grouped">
                <div class="control">
                    <button id="user-submit-button" class="button is-primary" @click.once="save">Submit</button>
                </div>
                <div class="control">
                    <a id="user-cancel-button" class="button" @click="cancel">Cancel</a>
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
    import { Vue, Component, Prop, Emit } from 'vue-property-decorator'
    import { User, UserClient } from '../../secretsanta-client';
    @Component
    export default class UserDetailsComponent extends Vue {
        @Prop()
        user: User;
        clonedUser: User = <User>{};

        constructor() {
            super();
        }

        mounted() {
            let tempUser = { ...this.user };
            this.clonedUser = <User>tempUser;
        }

        @Emit('user-saved')
        async save() {
            let userClient = new UserClient();
            if (this.clonedUser.id > 0) {
                await userClient.put(this.clonedUser.id, this.clonedUser);
            }
            else {
                await userClient.post(this.clonedUser);
            }
        }

        @Emit('user-saved')
        cancel() {
        }
    }
</script>