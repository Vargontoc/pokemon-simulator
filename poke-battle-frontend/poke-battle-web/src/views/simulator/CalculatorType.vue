<template>
    <div class="calculator-type">
        <div class="type-section">
            <h3>Defensa (Daño recibido)</h3>
            <calculator-type-defense />
        </div>

        <div class="type-section">
            <h3>Ataque (Daño realizado)</h3>
            <calculator-type-defense :is-attack="true" />
        </div>
    </div>
</template>

<script setup lang="ts">
import CalculatorTypeDefense from '@/components/simulator/CalculatorTypeDefense.vue';






</script>

<style lang="scss" scoped>
.calculator-type {
    display: flex;
    flex-direction: column;
    gap: 2rem;

    .type-section {
        border: 1px solid #ccc;
        border-radius: 8px;
        padding: 1rem;
        background: #fefefe;

        h3 {
            margin-bottom: 1rem;
            font-weight: bold;
            color: #333;
        }

        .type-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(110px, 1fr));
            gap: .5rem;
            margin-top: 1rem;

            .type-cell {
                display: flex;
                justify-content: space-between;
                align-items: center;
                padding: .4rem .6rem;
                border: 1px solid #ccc;
                border-radius: 6px;
                background: #f7f7f6;
                font-weight: 500;
            }

            &.x0 { background: #d1d5db; color: #333; }
            &.x0-5 { background: #fef3c7; color: #92400e;  }
            &.x1 { background: #e5e7eb; color: #1f2937; }
            &.x2 { background: #bbf7d0; color: #065f46;}
            &.x4 { background: #a7f3d0; border: 2px solid #059669; color: #065f46; font-weight: bold; }
        }
    }
}
</style>