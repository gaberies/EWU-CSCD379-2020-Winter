<template>
    <div>
        <div class="field">
            <label class="label">First Name</label>
            <div class="control">
                <input class="input" type="text" v-model="clonedGroup.title" />
            </div>
        </div>
        <div class="field is-grouped">
            <div class="control">
                <button id="submit" class="button is-primary" @click.once="saveGroup">Submit</button>
            </div>
            <div class="control">
                <a class="button is-light" @click="cancelEdit">Cancel</a>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { Vue, Component, Prop, Emit } from 'vue-property-decorator';
    import { Group } from '../../secretsanta-client';
    import { App } from '../../app';

    @Component
    export default class GroupDetailsComponent extends Vue {
        @Prop()
        group: Group;
        clonedGroup: Group = <Group>{};
        app: App.Main;

        constructor() {
            super();
            this.app = new App.Main();
        }

        mounted() {
            let tempGroup = { ...this.group };
            this.clonedGroup = <Group>tempGroup;
        }

        @Emit('group-saved')
        async saveGroup() {
            if (this.clonedGroup.id > 0) {
                await this.app.putGroup(this.clonedGroup);
            }
            else {
                await this.app.postGroup(this.clonedGroup);
            }
        }

        @Emit('group-saved')
        cancelEdit() {

        }
    }
</script>