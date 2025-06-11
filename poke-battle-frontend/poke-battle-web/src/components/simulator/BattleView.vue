<template>
<div class="battle-view">
    <div class="battle-view-content">
        <div class="battle-view-combat">
            <div class="battler battler-opponent">
                <div class="battler-info">
                    <div class="battler-name">{{ `${activeEnemy?.name} (Nv. ${activeEnemy?.level})` }}</div>
                    <div class="battler-hp-bar">
                        <div class="hp-fill" :style="{width: `${activeEnemy?.hpPerentage}%`}"></div>
                    </div>
                </div>

                <div class="opponent-party">
                    <div class="party-slot" v-for="b in enemyParty" :key="b.id" :title="b.wasOnBattle ? b.name: '???'">
                        <img class="party-icon unknown" v-if="!b.wasOnBattle" src="/src/assets/icons/battlers/000.png" alt="?"/>
                        <div v-else class="party-icon pokeball" :class="{ fainted: b.hp <= 0}"></div>
                    </div>
                </div>

                <div class="battler-image opponent">
                    <div class="shadow-circle"></div>
                    <img src="/src/assets/icons/battlers/000.png" />
                </div>

            </div>

            <div class="battler  battler-player">
                <div class="battler-info">
                    <div class="battler-name">{{ `${activePlayer?.name} (Nv. ${activePlayer?.level})` }}</div>
                    <div class="battler-hp-bar">
                        <div class="hp-fill" :style="{width: `${activePlayer?.hpPerentage}%`}"></div>
                    </div>
                </div>

                <div class="battler-image player">
                    <div class="shadow-circle"></div>
                    <img src="/src/assets/icons/battlers/000.png" />
                </div>
            </div>

            <div v-if="battleResult" class="battle-result">
                {{  battleResult === 'win' ? '¡Victoria!' : 'Derrota...' }}
            </div>

        </div>
        <div v-if="sendAction === false && battleResult === null" class="battle-view-battlers">
            <div class="battle-view-battler-info">
                <battler-card v-if="playerParty && playerParty.length"
                @switch-battler="onClickSwitch(index)"
                v-for="(battler, index) in playerParty"
                :key="battler.id"
                :battler="battler"/>
            </div>
            <div class="battle-view-battler-moves">
                <battler-move-card v-if="activePlayer && activePlayer.moves && activePlayer.moves.length"
                    @select-move="onClickAttack"
                    v-for=" move in activePlayer.moves"
                    :key="move.name"
                    :move="move" />
            </div>
        </div>
        <div v-else-if="playerParty.some(b => b.hp > 0) && sendAction" class="battle-view-waiting">
            <div class="waiting-card">
                <div class="spinner"></div>
                <p>Esperando respuesta del rival...</p>
            </div>
        </div>
    </div>
    <div class="battle-view-turn" v-if="turns && turns.length">
        <div class="battle-turn" v-for="(turn, i) in turns">
            <div class="battle-turn-header">Turn {{ i + 1 }}</div>
            <div class="battle-turn-message" v-for="m in turn.events">{{ m.message }}</div>
        </div>
    </div>
    
</div>
</template>
<script lang="ts" setup>
import BattlerInfoResponse from '@/models/simulator/BattlerInfoResponse';
import BattlerMoveCard from './BattleMoveCard.vue';
import BattlerCard from '@/components/simulator/BattlerCard.vue';
import client from '@/services/axios';
import BattleService from '@/services/BattleService';
import { computed, ref } from 'vue';
import { useRoute } from 'vue-router';
import type BattleTurn from '@/models/simulator/BattleTurn';
import type BattlerInfo from '@/models/simulator/BattlerInfo';

const battleService = new BattleService(client);

const router = useRoute();
const battleId = router.params.id as string;
const battleResponse = ref<BattlerInfoResponse>();
const turns = ref<BattleTurn[]>([])



const battleResult = computed(() : 'win' | 'lose' | null => {
        if(playerParty.value && playerParty.value.length && playerParty.value.every(p =>  p.hp <= 0)) return 'lose';
    else if(enemyParty.value && enemyParty.value.length &&  enemyParty.value.every(p =>  p.hp <= 0)) return'win';
    return null;
})

const sendAction = ref<boolean>(false);
loadBattleInfo();

function loadBattleInfo() {
    if (battleId) 
    {
        battleService.getBattleInfo(battleId).then((response: BattlerInfoResponse) => {
            battleResponse.value = response;
            sendAction.value = false;
        }).catch((error: any) => {
            console.error('Error fetching battle info:', error);
        });
    } else {
        console.warn('No battle ID provided.');
    }
}

const playerParty = computed(() : BattlerInfo[] => {
    if(battleResponse.value && battleResponse.value.battlers && battleResponse.value.battlers.length)
        return battleResponse.value.battlers.filter(x => x.isPlayer === true);
    return []; 
})

const enemyParty = computed(() : BattlerInfo[] => {
    if(battleResponse.value && battleResponse.value.battlers && battleResponse.value.battlers.length)
        return battleResponse.value.battlers.filter(x => x.isPlayer === false);
    return []; 
})

const activePlayer = computed(() : BattlerInfo | undefined => {
    return playerParty.value.find(x => x.isActive === true)
})

const activeEnemy = computed(() : BattlerInfo | undefined => {
    return enemyParty.value.find(x => x.isActive === true)
})



function onClickAttack(attack: string) {
    sendAction.value = true;
    battleService.attack(battleId, attack).then((r) => {
        turns.value.push(r);
        loadBattleInfo();
        
    })
}

function onClickSwitch(id: number) {
    
    sendAction.value = true;
    battleService.switch(battleId, ''+id).then((r) => {
        turns.value.push(r);
        loadBattleInfo();
    })
}
</script>
<style lang="scss" scoped>
.battle-view {
    height: 75%;
    width:  95%;
    padding: .5rem;
    display: flex;
}

.battle-view-content {
    width: 600px;
    height: 98%;
    padding: .5rem;
}

.battle-view-turn {
    max-height: 300px;
    overflow-y: auto;
    padding: 1rem;
    background: #f9f9f9;
    border: 1px solid #ddd;
    border-radius: 8px;
    font-size: .95rem;
    color: #333;

    .battle-turn {
        margin-bottom: 1rem;
        padding-bottom: .5rem;
        border-bottom: 1px solid #ccc;

        .battle-turn-header {
            font-weight: bold;
            font-size: 1rem;
            margin-bottom: .5rem;
            color: #2a6ebc;
        }

        .battle-turn-message {
            padding: .25rem .5rem;
            margin-left: 1rem;
            background: #fff;
            border-left: 4px solid #2a6ebc;
            margin-bottom: .25rem;
            border-radius: 4px;
        }
    }
}

.battle-view-combat {
    width: 98%;
    height: 70%;
    padding: 1rem;
    border-radius: 10px;
    position: relative;
    display: flex;
    background: linear-gradient(to bottom, #dbefff, #ffffff);
    overflow: hidden;
    flex-direction: column;
    justify-content: space-between;

    .battle-result {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        font-size: 2rem;
        font-weight: bold;
        background-color: rgba(255,255,255, .85);
        padding: 1rem 2rem;
        border-radius: 12px;
        box-shadow: 0 4px 16px rgba($color: #000000, $alpha: .3);
        color: #2a6ebc;
        text-align: center;
        z-index: 100;

        &::before{
            content: '';
            position: absolute;
            inset: 0;
            border-radius: 12px;
            backdrop-filter: blur(2px);
        }
    }


    .battler {
        position: relative;
        display: flex;
        flex-direction: column;
        align-items: center;

        &.battler-opponent {
            align-self: flex-end;

            .opponent-party {
                margin-top: -.1rem;
                margin-right: 3.5rem;
                display: flex;
                gap: .25rem;

                .party-slot {
                    width: 20px;
                    height: 20px;
                    position: relative;

                    .party-icon {
                        width: 100%;
                        height: 100%;
                        border-radius: 50%;
                        display: block;

                        &.pokeball {
                            background: radial-gradient(white 30%, red 60%, black 100%);
                            border: 1px solid black;
                        }

                        &.pokeball.fainted {
                            background: radial-gradient(white 30%, gray 60%, black 100%);
                            opacity: .5;
                        }

                        &.unknown  {
                            object-fit: cover;
                            opacity: .75;
                        }
                    }
                }


                .pokeball.fainted {
                    background:  radial-gradient(white 30%, gray 60%, black 100%);
                    opacity: .5;
                    cursor: default;
                }
            }
        }

        &.battler-player {
            align-self: flex-start;
        }

        &.battler-opponent, &.battler-player {
            .shadow-circle {
                position: absolute;
                bottom: .2rem;
                left: 50%;
                transform: translateX(-50%);
                width: 60%;
                height: 12px;
                background: rgba(0,0,0, .2);
                border-radius: 50%;
                filter: blur(2px);
                z-index: 0;
            }
            .battler-image {
                position: absolute;
                width: 96px;
                height: 96px;
                display: flex;
                justify-content: center;
                align-items: flex-end;

                img {
                    width: 100%;
                    position: relative;
                    z-index: 1;
                    height: auto;
                    image-rendering: pixelated;
                }
                &.opponent {
                    top: 4rem;
                    right: 5rem;
                }
                &.player {
                    bottom: 4rem;
                    left: 5rem;
                }
            }

            .battler-info {
                background: white;
                border: 1px solid #ccc;
                border-radius: 6px;
                padding: .5rem;
                width: 180px;
                text-align: center;

                .battler-name {
                    font-weight: bold;
                    margin-bottom: .25rem;
                }

                .battler-hp-bar {
                    width: 100%;
                    height: 10px;
                    background: #eee;
                    border-radius: 5px;
                    overflow: hidden;

                    .hp-fill {
                        height: 100%;
                        transition: width .4 ease;
                        background: linear-gradient(to right, #4caf50, #81c784);
                    }
                }
            }
        }
    }
}


.battle-view-waiting {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 180px;
    text-align: center;
    background: #f3f6fc;
    border: 1px dashed #aaa;
    border-radius: 8px;

    .waiting-card {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: .75rem;
        color: #333;
        font-size: 1rem;

        .spinner {
            width: 24px;
            height: 24px;
            border: 3px solid #ccc;
            border-top-color: #2a6ebc;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
    }
}

@keyframes spin {
    to {
        transform: rotate(360deg);
    }
}

.battle-view-battlers {
    width: 98%;
    margin-top: 1%;
    height: 22%;
    padding: .5rem;
}

.battle-view-battler-info {
    width: 100%;
    display: flex;
    flex-wrap: row;
    gap: .5rem;
}
.battle-view-battler-moves {
    width: 100%;
    margin-top: .5rem;
    height: 60%;
    display: flex;
    flex-wrap: row;
    gap: .5rem;
}

</style>