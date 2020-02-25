"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var page_1 = require("./page");
var secretsanta_client_1 = require("./secretsanta-client");
var app = new page_1.App();
app.deleteAllGifts();
app.addAllGifts(giftsArray());
generateGifts(app);
function generateGifts(app) {
    app.getAllGifts().then(function (giftsArray) {
        giftsArray.forEach(function (gift) {
            document.getElementById("giftsPage").append(convert(gift));
        });
    });
}
function convert(gift) {
    return "<li>Title:" + gift.title + " Description:" + gift.description + " Url:" + gift.url + "</li>";
}
function giftsArray() {
    var giftsArray = new secretsanta_client_1.Gift[7];
    giftsArray[0] = new secretsanta_client_1.Gift(createGift("Title", "Description", "Url"));
    giftsArray[1] = new secretsanta_client_1.Gift(createGift("box", "boring", "www.amazon.com"));
    giftsArray[2] = new secretsanta_client_1.Gift(createGift("bike", "fun and quick", "www.bikestore.com"));
    giftsArray[3] = new secretsanta_client_1.Gift(createGift("socks", "warm", "www.sockstore.com"));
    giftsArray[4] = new secretsanta_client_1.Gift(createGift("shoe", "flashy", "www.shoes.com"));
    giftsArray[5] = new secretsanta_client_1.Gift(createGift("TV", "expensive", "www.bestbuy.com"));
    giftsArray[6] = new secretsanta_client_1.Gift(createGift("computer", "more expensive", "www.amazon.com"));
    return giftsArray;
}
function createGift(title, description, url) {
    var toReturn = new secretsanta_client_1.Gift();
    toReturn.title = title;
    toReturn.description = description;
    toReturn.url = url;
    return toReturn;
}
//# sourceMappingURL=list-Gifts.js.map