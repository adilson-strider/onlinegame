import { Socket } from 'socket.io';
import { Character_Plain } from '../src/api/character/content-types/character/character';
import { User_Plain } from '../src/common/schemas-to-ts/User';

export type QueuePlayer = {
    userId: number,
    characterId: number,
    socket: Socket
}

type BattlePlayer = {
    id: number,
    username: string,
    socket: Socket,
    character: Character_Plain
}

class Battle{
    player1: BattlePlayer;
    player2: BattlePlayer;

    constructor(player1: BattlePlayer, player2: BattlePlayer){
        this.player1 = player1;
        this.player2 = player2;
    }
}

let queue: QueuePlayer[] = [];

export const addToQueue = async (player:QueuePlayer) => {
    
    if(queue.length > 0) {
        const opponent = queue.shift();

        const player1FromStrapi:User_Plain = await strapi.db.query("plugin::users-permissions.user")
        .findOne({
            where: {id: player.userId}
        })

        const player2FromStrapi:User_Plain = await strapi.db.query("plugin::users-permissions.user")
        .findOne({
            where: {id: opponent.userId}
        })

        const player1Character = await strapi.entityService.findOne("api::character.character", player.characterId,
            { populate: [ ] }) as Character_Plain

        const player2Character = await strapi.entityService.findOne("api::character.character", opponent.characterId,
            { populate: [ ] }) as Character_Plain

        const player1: BattlePlayer = {
            id: player.userId,
            username: player1FromStrapi.username,
            socket: player.socket,
            character: player1Character,
        };

        const player2: BattlePlayer = {
            id: opponent.userId,
            username: player2FromStrapi.username,
            socket: opponent.socket,
            character: player2Character,
        };

        console.log(player1, player2);

        console.log("Battle started between " + player1.username + " and " + player2.username);

        const battle = new Battle(player1, player2);

    } else {
        queue.push(player);
    }
}