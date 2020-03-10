<template>
    <div>
        <h2>Groups</h2>
        <button id="create-new-group-button" class="button is-secondary" @click="create()">Create New</button>
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Title</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr id="`${group.id}`" v-for="group in groups" :key="group.id">
                    <td>{{group.id}}</td>
                    <td>{{group.title}}</td>
                    <td>
                        <button id="`${group.id}-edit-group-button`" class="button is-primary" @click="edit(group)">Edit</button>
                        <button id="`${group.id}-delete-group-button`" class="button" @click="deleteGroup(group)">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <group-details-component v-if="selectedGroup != null"
                                :group="selectedGroup"
                                @group-saved="refresh()"></group-details-component>
    </div>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import { Group, GroupClient } from '../../secretsanta-client';
    import GroupDetailsComponent from './groupDetailsComponent.vue';

    @Component({
        components: {
            GroupDetailsComponent
        }
    })
    export default class GroupsComponent extends Vue {
        groups: Group[] = null;
        selectedGroup: Group = null;
        groupClient: GroupClient;

        constructor() {
            super();
            this.groupClient = new GroupClient();
        }

        async mounted() {
            this.groups = await this.groupClient.getAll()
        }

        create() {
            this.selectedGroup = <Group>{};
        }

        edit(group: Group) {
            this.selectedGroup = group;
        }

        async deleteGroup(group: Group) {
            if (confirm(`Are you sure you want to delete ${group.title}?`)) {
                await this.groupClient.delete(group.id);
                this.refresh();
            }
        }

        async refresh() {
            this.groups = await this.groupClient.getAll();
            this.selectedGroup = null;
        }
    }
</script>