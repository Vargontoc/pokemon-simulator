import { createRouter, createWebHistory } from "vue-router";
import LAbilitiesManagementView from "@/views/crud/abilities/LAbilitiesManagementView.vue";
import LTypesManagementView from "@/views/crud/types/LTypesManagementView.vue";
import BattleSimulatorView from "@/views/simulator/BattleSimulatorView.vue";
import LMoveManagementView from "@/views/crud/moves/LMoveManagementView.vue";
import ExperienceSimulatorView from "../views/simulator/ExperienceSimulatorView.vue";   


const routes = [
    { path: '/', redirect: '/abilities' },
    { path: '/abilities', component: LAbilitiesManagementView },
    { path: '/types', component: LTypesManagementView },
    { path: '/moves', component: LMoveManagementView},
    { path: '/simulator-battle', component: BattleSimulatorView},
    { path: '/simulator-experience', component: ExperienceSimulatorView},
    
]

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;