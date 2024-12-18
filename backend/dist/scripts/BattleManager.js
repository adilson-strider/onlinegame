"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.addToQueue = void 0;
class Battle {
    constructor(player1, player2) {
        this.player1 = player1;
        this.player2 = player2;
    }
}
let queue = [];
const addToQueue = async (player) => {
    if (queue.length > 0) {
        const opponent = queue.shift();
        const player1FromStrapi = await strapi.db.query("plugin::users-permissions.user")
            .findOne({
            where: { id: player.userId }
        });
        const player2FromStrapi = await strapi.db.query("plugin::users-permissions.user")
            .findOne({
            where: { id: opponent.userId }
        });
        const player1Character = await strapi.entityService.findOne("api::character.character", player.characterId, { populate: [] });
        const player2Character = await strapi.entityService.findOne("api::character.character", opponent.characterId, { populate: [] });
        const player1 = {
            id: player.userId,
            username: player1FromStrapi.username,
            socket: player.socket,
            character: player1Character,
        };
        const player2 = {
            id: opponent.userId,
            username: player2FromStrapi.username,
            socket: opponent.socket,
            character: player2Character,
        };
        console.log(player1, player2);
        console.log("Battle started between " + player1.username + " and " + player2.username);
        const battle = new Battle(player1, player2);
    }
    else {
        queue.push(player);
    }
};
exports.addToQueue = addToQueue;
