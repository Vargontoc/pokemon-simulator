import { createRouter, createWebHistory } from "vue-router";
import LAbilitiesManagementView from "@/views/crud/abilities/LAbilitiesManagementView.vue";
import LTypesManagementView from "@/views/crud/types/LTypesManagementView.vue";
import BattleSimulatorView from "@/views/simulator/BattleSimulatorView.vue";
import LMoveManagementView from "@/views/crud/moves/LMoveManagementView.vue";  
import BattleView from "@/components/simulator/BattleView.vue";
import ErrorView from "@/views/ErrorView.vue";
import SpecieManagementView from "@/views/crud/species/SpecieManagementView.vue";
import ItemsManagementView from "@/views/crud/items/ItemsManagementView.vue";
import CalculatorsView from "@/views/simulator/CalculatorsView.vue";
import TrainersManagementView from "@/views/crud/trainers/TrainersManagementView.vue";
import EncountersManagement from "@/views/crud/encounters/EncountersManagement.vue";


const routes = [
    { path: '/', redirect: '/abilities' },
    { path: '/abilities', component: LAbilitiesManagementView },
    { path: '/species', component: SpecieManagementView},
    { path: '/types', component: LTypesManagementView },
    { path: '/items', component: ItemsManagementView},
    { path: '/moves', component: LMoveManagementView},
    { path: '/trainers', component: TrainersManagementView},
    { path: '/simulator-battle', component: BattleSimulatorView},
    { path: '/calculators', component: CalculatorsView},
    { path: '/encounters', component: EncountersManagement},
    { name: 'Battle', path: '/battle/:id', component: BattleView, props: true },
    { name: 'Error', path: '/error', component: ErrorView, meta: { layout: 'none'}}
    
]

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;