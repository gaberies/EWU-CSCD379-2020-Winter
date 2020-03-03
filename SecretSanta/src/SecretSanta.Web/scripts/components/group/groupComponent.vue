<template>
    <div>
        <button class="button" @click="createGroup()">Create New</button>
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Title</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="group in groups" :id="group.id">
                    <td>{{group.id}}</td>
                    <td>{{group.title}}</td>
                    <td>
                        <button class="button" @click='setGroup(group)'>Edit</button>
                        <button class="button" @click='deleteGroup(group)'>Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <group-details-component v-if="selectedGroup != null"
                                :group="selectedGroup"
                                @group-saved="refreshGroups()"></group-details-component>
    </div>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import { Group } from '../../secretsanta-client';
    import { App } from '../../app';
    import GroupDetailsComponent from './groupDetailsComponents.vue';

    @Component({
        components: {
            GroupDetailsComponent
        }
    })

    export default class GroupsComponent extends Vue {
        groups: Group[] = null;
        selectedGroup: Group = null;
        app: App.Main = new App.Main();

        async loadGroups() {
            this.groups = await this.app.getAllGroups();
        }

        createGroup() {
            this.selectedGroup = <Group>{};
        }

        async mounted() {
            await this.loadGroups();
        }

        setGroup(group: Group) {
            this.selectedGroup = group;
        }

        async refreshGroups() {
            this.selectedGroup = null;
            await this.loadGroups();
            this.$forceUpdate();
        }

        async deleteGroup(group: Group) {
            if (confirm(`Are you sure you want to delete ${group.title}`)) {
                await this.app.deleteGroup(group);
            }
            await this.refreshGroups();
        }
    }
</script>