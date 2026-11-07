<script setup lang="ts">
import { computed, reactive } from 'vue'
import {
  Search,
  Filter,
  RotateCcw,
} from 'lucide-vue-next'
import type { Movie } from '../types/movie'

const props = defineProps<{
  movies: Movie[]
}>()

const emit = defineEmits<{
  filter: [movies: Movie[]]
}>()

const filters = reactive({
  titulo: '',
  genero: '',
  director: '',
  idioma: '',
  pais: '',
  clasificacion_edad: '',
  calificacionMin: '',
  calificacionMax: '',
})

const normalize = (value: string) =>
  value
    .replace(/\?/g, '')
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/\s+/g, ' ')
    .trim()
    .toLowerCase()

const splitValues = (value: string) =>
  value
    .split(',')
    .map((item) => item.replace(/\?/g, '').replace(/\s+/g, ' ').trim())
    .filter(Boolean)

const uniqueValues = (values: string[]) => {
  const unique = new Map<string, string>()

  values.flatMap(splitValues).forEach((value) => {
    const key = normalize(value)
    if (key && !unique.has(key)) unique.set(key, value)
  })

  return [...unique.values()]
}

const includesValue = (value: string, selected: string) =>
  splitValues(value).some((item) => normalize(item) === normalize(selected))

const genres = computed(() => {
  return uniqueValues(props.movies.map((movie) => movie.genero))
})

const directors = computed(() => {
  return uniqueValues(props.movies.map((movie) => movie.director))
})

const languages = computed(() => {
  return uniqueValues(props.movies.map((movie) => movie.idioma))
})

const countries = computed(() => {
  return uniqueValues(props.movies.map((movie) => movie.pais))
})

const ageRatings = computed(() => {
  return uniqueValues(props.movies.map((movie) => movie.clasificacion_edad))
})

const applyFilters = () => {
  const filteredMovies = props.movies.filter((movie) => {
    const titleMatch = normalize(movie.titulo).includes(normalize(filters.titulo))

    const genreMatch =
      !filters.genero || includesValue(movie.genero, filters.genero)

    const directorMatch =
      !filters.director || includesValue(movie.director, filters.director)

    const languageMatch =
      !filters.idioma || includesValue(movie.idioma, filters.idioma)

    const countryMatch =
      !filters.pais || includesValue(movie.pais, filters.pais)

    const ageMatch =
      !filters.clasificacion_edad ||
      includesValue(movie.clasificacion_edad, filters.clasificacion_edad)

    const minRatingMatch =
      filters.calificacionMin === '' ||
      movie.calificacion >= Number(filters.calificacionMin)

    const maxRatingMatch =
      filters.calificacionMax === '' ||
      movie.calificacion <= Number(filters.calificacionMax)

    return (
      titleMatch &&
      genreMatch &&
      directorMatch &&
      languageMatch &&
      countryMatch &&
      ageMatch &&
      minRatingMatch &&
      maxRatingMatch
    )
  })

  emit('filter', filteredMovies)
}

const clearFilters = () => {
  filters.titulo = ''
  filters.genero = ''
  filters.director = ''
  filters.idioma = ''
  filters.pais = ''
  filters.clasificacion_edad = ''
  filters.calificacionMin = ''
  filters.calificacionMax = ''

  emit('filter', props.movies)
}
</script>

<template>
  <section class="filters">

    <div class="filters-header">
      <div class="filters-title">
        <Filter :size="20" />

        <div>
          <h2>Buscar y filtrar</h2>
          <p>Encuentra una película según sus características.</p>
        </div>
      </div>

      <button
        class="clear-button"
        type="button"
        @click="clearFilters"
      >
        <RotateCcw :size="16" />
        Limpiar filtros
      </button>
    </div>

    <div class="search-box">
      <Search :size="20" />

      <input
        v-model="filters.titulo"
        type="text"
        placeholder="Buscar película por título..."
        @input="applyFilters"
      />
    </div>

    <div class="filter-grid">

      <div class="filter-group">
        <label for="genero">
          Género
        </label>

        <select
          id="genero"
          v-model="filters.genero"
          @change="applyFilters"
        >
          <option value="">Todos</option>

          <option
            v-for="genre in genres"
            :key="genre"
            :value="genre"
          >
            {{ genre }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="director">
          Director
        </label>

        <select
          id="director"
          v-model="filters.director"
          @change="applyFilters"
        >
          <option value="">Todos</option>

          <option
            v-for="director in directors"
            :key="director"
            :value="director"
          >
            {{ director }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="idioma">
          Idioma
        </label>

        <select
          id="idioma"
          v-model="filters.idioma"
          @change="applyFilters"
        >
          <option value="">Todos</option>

          <option
            v-for="language in languages"
            :key="language"
            :value="language"
          >
            {{ language }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="pais">
          País
        </label>

        <select
          id="pais"
          v-model="filters.pais"
          @change="applyFilters"
        >
          <option value="">Todos</option>

          <option
            v-for="country in countries"
            :key="country"
            :value="country"
          >
            {{ country }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="clasificacion">
          Clasificación
        </label>

        <select
          id="clasificacion"
          v-model="filters.clasificacion_edad"
          @change="applyFilters"
        >
          <option value="">Todas</option>

          <option
            v-for="rating in ageRatings"
            :key="rating"
            :value="rating"
          >
            {{ rating }}
          </option>
        </select>
      </div>

      <div class="rating-group">
        <label>
          Calificación
        </label>

        <div class="rating-inputs">
          <input
            v-model="filters.calificacionMin"
            type="number"
            min="0"
            max="10"
            step="0.1"
            placeholder="Mín."
            @input="applyFilters"
          />

          <span>—</span>

          <input
            v-model="filters.calificacionMax"
            type="number"
            min="0"
            max="10"
            step="0.1"
            placeholder="Máx."
            @input="applyFilters"
          />
        </div>
      </div>

    </div>

  </section>
</template>

<style scoped>
.filters {
  margin-bottom: 35px;
  padding: 25px;
  border: 1px solid #e5e7eb;
  border-radius: 14px;
  background: #ffffff;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
}

.filters-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 20px;
  margin-bottom: 20px;
}

.filters-title {
  display: flex;
  align-items: center;
  gap: 12px;
}

.filters-title > svg {
  color: #2563eb;
}

.filters-title h2 {
  margin: 0 0 4px;
  color: #111827;
  font-size: 18px;
  font-weight: 800;
}

.filters-title p {
  margin: 0;
  color: #6b7280;
  font-size: 13px;
}

.clear-button {
  display: flex;
  align-items: center;
  gap: 7px;
  padding: 9px 13px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background: #ffffff;
  color: #374151;
  font-size: 13px;
  font-weight: 600;
}

.clear-button:hover {
  background: #f3f4f6;
}

.search-box {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
  padding: 12px 14px;
  border: 1px solid #d1d5db;
  border-radius: 9px;
  background: #ffffff;
}

.search-box:focus-within {
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
}

.search-box svg {
  flex-shrink: 0;
  color: #6b7280;
}

.search-box input {
  width: 100%;
  border: none;
  outline: none;
  color: #111827;
  background: transparent;
  font-size: 15px;
}

.search-box input::placeholder {
  color: #9ca3af;
}

.filter-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.filter-group,
.rating-group {
  display: flex;
  flex-direction: column;
  gap: 7px;
}

.filter-group label,
.rating-group label {
  color: #374151;
  font-size: 13px;
  font-weight: 700;
}

.filter-group select,
.rating-inputs input {
  width: 100%;
  padding: 10px 11px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  outline: none;
  background: #ffffff;
  color: #374151;
  font-size: 14px;
}

.filter-group select:focus,
.rating-inputs input:focus {
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
}

.rating-inputs {
  display: flex;
  align-items: center;
  gap: 8px;
}

.rating-inputs span {
  color: #9ca3af;
}

@media (max-width: 850px) {
  .filter-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 600px) {
  .filters-header {
    align-items: flex-start;
    flex-direction: column;
  }

  .filter-grid {
    grid-template-columns: 1fr;
  }

  .clear-button {
    width: 100%;
    justify-content: center;
  }
}
</style>
