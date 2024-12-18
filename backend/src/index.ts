// import type { Core } from '@strapi/strapi';
import * as socketio from 'socket.io';
import jwt from 'jsonwebtoken';
import { QueuePlayer, addToQueue } from '../scripts/BattleManager';

export default {
  /**
   * An asynchronous register function that runs before
   * your application is initialized.
   *
   * This gives you an opportunity to extend code.
   */
  register(/* { strapi }: { strapi: Core.Strapi } */) {},

  /**
   * An asynchronous bootstrap function that runs before
   * your application gets started.
   *
   * This gives you an opportunity to set up your data model,
   * run jobs, or perform some special logic.
   */
  bootstrap({ strapi }) {
    const io = new  socketio.Server(strapi.server.httpServer);
    
    io.on('connection', (socket) => {
      console.log('a user connected');

      socket.on('searchBattle', (data) => {
        console.log(data);
        const myJwt = data.jwt;
        const charId = data.characterId; // Verificar depois

        const jwtSecret = process.env.JWT_SECRET
        const decoded = jwt.verify(myJwt, jwtSecret) as jwt.JwtPayload;
        const userId = decoded.id;

        // Matchmaking logic
        const playerObjForQueue:QueuePlayer = {
          userId: userId,
          characterId: charId,
          socket: socket
        }

        // Add player to queue
        addToQueue(playerObjForQueue);
      });

      socket.on('sendTurn', (data) => {
        console.log('sendTurn', data);
      });
    });
  },
};
