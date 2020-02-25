import { App } from "./page";
import { Gift, IGift } from "./secretsanta-client";

const app = new App();

app.deleteAllGifts();
app.addAllGifts(giftsArray());
generateGifts(app);

function generateGifts(app: App) {
    app.getAllGifts().then((giftsArray: Gift[]) => {
        giftsArray.forEach((gift) => {
            document.getElementById("giftsPage").append(convert(gift));
        });
    });
}

function convert(gift: Gift): string {
    return `<li>Title:${gift.title} Description:${gift.description} Url:${gift.url}</li>`;
}

function giftsArray(): Gift[] {
    let giftsArray: Gift[] = new Gift[7];
    giftsArray[0] = new Gift(createGift("Title", "Description", "Url"));
    giftsArray[1] = new Gift(createGift("box", "boring", "www.amazon.com"));
    giftsArray[2] = new Gift(createGift("bike", "fun and quick", "www.bikestore.com"));
    giftsArray[3] = new Gift(createGift("socks", "warm", "www.sockstore.com"));
    giftsArray[4] = new Gift(createGift("shoe", "flashy", "www.shoes.com"));
    giftsArray[5] = new Gift(createGift("TV", "expensive", "www.bestbuy.com"));
    giftsArray[6] = new Gift(createGift("computer", "more expensive", "www.amazon.com"));
    return giftsArray;
}

function createGift(title: string, description: string, url: string): IGift {
    let toReturn: IGift = new Gift();
    toReturn.title = title;
    toReturn.description = description;
    toReturn.url = url;
    return toReturn;
}