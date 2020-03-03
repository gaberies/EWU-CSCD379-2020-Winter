<template>
    <div>
        <div class="field">
            <label class="label">Title</label>
            <div class="control">
                <input class="input" type="text" v-model="clonedGift.title" />
            </div>
        </div>
        <div class="field">
            <label class="label">Description</label>
            <div class="control">
                <input class="input" type="text" v-model="clonedGift.description" />
            </div>
        </div>
        <div class="field">
            <label class="label">Url</label>
            <div class="control">
                <input class="input" type="url" v-model="clonedGift.url" />
            </div>
        </div>
        <div class="field is-grouped">
            <div class="control">
                <button id="submit" class="button is-primary" @click.once="saveGift">Submit</button>
            </div>
            <div class="control">
                <a class="button is-light" @click="cancelEdit">Cancel</a>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { Vue, Component, Prop, Emit } from 'vue-property-decorator';
    import { Gift } from '../../secretsanta-client';
    import { App } from '../../app';
    
    @Component
    export default class GiftDetailsComponent extends Vue {
        @Prop()
        gift: Gift;
        clonedGift: Gift = <Gift>{};
        app: App.Main;

        constructor() {
            super();
            this.app = new App.Main();
        }

        mounted() {
            let tempGift = { ...this.gift };
            this.clonedGift = <Gift>tempGift;
        }

        @Emit('gift-saved')
        async saveGift() {
            if (this.clonedGift.id > 0) {
                await this.app.putGift(this.clonedGift);
            }
            else {
                await this.app.postGift(this.clonedGift);
            }
        }

        @Emit('gift-saved')
        cancelEdit() {
            this.clonedGift.url
        }
    }
</script>