<template>
    <aside class="sidebar">
        <nav class="nav-menu">
            <ul>
                <li v-for="item in mainMenu" :key="item.path">
                    <RouterLink :to="item.path!">{{  item.name }}</RouterLink>
                    <ul v-if="item.children">
                        <li v-for="child in item.children" :key="child.path">
                            <RouterLink :to="child.path">{{  child.name }}</RouterLink>
                        </li>
                    </ul>
                </li>
            </ul>
        </nav>
    </aside>
</template>
<script setup lang="ts">
import { mainMenu } from '@/routes/menu';




</script>

<style lang="scss">
.sidebar {
  width: 220px;
  background-color: #2c3e50;
  color: white;
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: flex-start; /* Alinea el contenido a la izquierda */
}

.nav-menu {
  width: 100%;
}

.sidebar ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.sidebar li {
  margin: 8px 0;
}

.sidebar a {
  color: white;
  text-decoration: none;
}

.sidebar a.router-link-active {
  font-weight: bold;
}
</style>