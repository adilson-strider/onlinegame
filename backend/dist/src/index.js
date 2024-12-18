"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
// import type { Core } from '@strapi/strapi';
const socketio = __importStar(require("socket.io"));
const jsonwebtoken_1 = __importDefault(require("jsonwebtoken"));
const BattleManager_1 = require("../scripts/BattleManager");
exports.default = {
    /**
     * An asynchronous register function that runs before
     * your application is initialized.
     *
     * This gives you an opportunity to extend code.
     */
    register( /* { strapi }: { strapi: Core.Strapi } */) { },
    /**
     * An asynchronous bootstrap function that runs before
     * your application gets started.
     *
     * This gives you an opportunity to set up your data model,
     * run jobs, or perform some special logic.
     */
    bootstrap({ strapi }) {
        const io = new socketio.Server(strapi.server.httpServer);
        io.on('connection', (socket) => {
            console.log('a user connected');
            socket.on('searchBattle', (data) => {
                console.log(data);
                const myJwt = data.jwt;
                const charId = data.characterId; // Verificar depois
                const jwtSecret = process.env.JWT_SECRET;
                const decoded = jsonwebtoken_1.default.verify(myJwt, jwtSecret);
                const userId = decoded.id;
                // Matchmaking logic
                const playerObjForQueue = {
                    userId: userId,
                    characterId: charId,
                    socket: socket
                };
                // Add player to queue
                (0, BattleManager_1.addToQueue)(playerObjForQueue);
            });
            socket.on('sendTurn', (data) => {
                console.log('sendTurn', data);
            });
        });
    },
};
