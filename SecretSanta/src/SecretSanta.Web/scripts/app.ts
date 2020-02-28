import {
    GiftClient,
    Gift,
    UserClient,
    User,
    GroupClient,
    Group
} from './secretsanta-client';

export module App {
    export class Main {
        giftClient: GiftClient;
        userClient: UserClient;
        groupClient: GroupClient;
        createdUser: User;

        constructor() {
            this.giftClient = new GiftClient('https://localhost:44388');
            this.userClient = new UserClient('https://localhost:44388');
            this.groupClient = new GroupClient('https://localhost:44388');
        }

        async getAllGifts(): Promise<Gift[]> {
            return await this.giftClient.getAll();
        }

        async getGift(id: number): Promise<Gift> {
            return this.giftClient.get(id);
        }

        async postGift(gift: Gift): Promise<Gift> {
            return await this.giftClient.post(gift);
        }

        async putGift(gift: Gift): Promise<Gift> {
            return await this.giftClient.put(gift.id, gift);
        }

        async deleteGift(gift: Gift): Promise<void> {
            this.giftClient.delete(gift.id);
        }

        async deleteAllGifts(): Promise<void> {
            const gifts: Gift[] = await this.getAllGifts();
            for (let gift of gifts) {
                await this.deleteGift(gift);
            }
        }

        async getAllUsers(): Promise<User[]> {
            return await this.userClient.getAll();
        }

        async getUser(id: number): Promise<User> {
            return this.userClient.get(id);
        }

        async postUser(user: User): Promise<User> {
            return await this.userClient.post(user);
        }

        async putUser(user: User): Promise<User> {
            return await this.userClient.put(user.id, user);
        }

        async deleteUser(user: User): Promise<void> {
            this.giftClient.delete(user.id);
        }

        async deleteAllUsers(): Promise<void> {
            const users: User[] = await this.getAllUsers();
            for (let user of users) {
                await this.deleteUser(user);
            }
        }

        async getAllGroups(): Promise<Group[]> {
            return await this.groupClient.getAll();
        }

        async getGroup(id: number): Promise<Group> {
            return this.groupClient.get(id);
        }

        async postGroup(group: Group): Promise<Group> {
            return await this.groupClient.post(group);
        }

        async putGroup(group: Group): Promise<Group> {
            return await this.groupClient.put(group.id, group);
        }

        async deleteGroup(group: Group): Promise<void> {
            this.groupClient.delete(group.id);
        }

        async deleteAllGroup(): Promise<void> {
            const groups: Group[] = await this.getAllGroups();
            for (let group of groups) {
                await this.deleteGroup(group);
            }
        }
    }
}