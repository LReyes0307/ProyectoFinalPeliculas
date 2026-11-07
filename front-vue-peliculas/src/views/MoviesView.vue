<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import Navbar from '../components/Navbar.vue'
import MovieCard from '../components/MovieCard.vue'
import MovieFilters from '../components/MovieFilters.vue'
import { useMoviesStore } from '../stores/movies'
import type { Movie } from '../types/movie'

const moviesStore = useMoviesStore()

const filteredMovies = ref<Movie[]>([])

const currentPage = ref(1)
const moviesPerPage = 6

const updateMovies = (movies: Movie[]) => {
  filteredMovies.value = movies
  currentPage.value = 1
}

onMounted(async () => {
  try {
    await moviesStore.loadMovies()
    updateMovies(moviesStore.movies)
  } catch {
    filteredMovies.value = []
  }
})

const totalPages = computed(() => {
  return Math.ceil(filteredMovies.value.length / moviesPerPage)
})

const paginatedMovies = computed(() => {
  const start = (currentPage.value - 1) * moviesPerPage
  const end = start + moviesPerPage

  return filteredMovies.value.slice(start, end)
})

const pages = computed(() => {
  return Array.from(
    { length: totalPages.value },
    (_, index) => index + 1,
  )
})

const goToPage = (page: number) => {
  if (page < 1 || page > totalPages.value) return

  currentPage.value = page
}

const previousPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--
  }
}

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++
  }
}
</script>

<template>
  <div class="movies-page">
    <Navbar />

    <main class="movies-content">
      <section class="page-header">
        <h1>Películas recomendadas</h1>

        <p>
          Descubre qué películas valen la pena ver.
        </p>
      </section>

      <MovieFilters
        :movies="moviesStore.movies"
        @filter="updateMovies"
      />

      <section
        v-if="paginatedMovies.length > 0"
        class="movies-grid"
      >
        <MovieCard
          v-for="movie in paginatedMovies"
          :key="movie.id"
          :movie="movie"
        />
      </section>

      <p
        v-else
        class="no-results"
      >
        No encontramos películas con esos filtros.
      </p>

      <nav class="pagination" aria-label="Paginación de películas">
        <button
          class="pagination-button"
          :disabled="currentPage === 1"
          @click="previousPage"
        >
          Anterior
        </button>

        <button
          v-for="page in pages"
          :key="page"
          class="pagination-button page-number"
          :class="{ active: currentPage === page }"
          @click="goToPage(page)"
        >
          {{ page }}
        </button>

        <button
          class="pagination-button"
          :disabled="currentPage === totalPages"
          @click="nextPage"
        >
          Siguiente
        </button>
      </nav>
    </main>
  </div>
</template>

<style scoped>
.movies-page {
  min-height: 100vh;
  background: #f5f6f8;
}

.movies-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 25px;
}

.page-header {
  margin-bottom: 30px;
}

.page-header h1 {
  margin: 0 0 8px;
  color: #111827;
  font-size: 32px;
  font-weight: 800;
}

.page-header p {
  margin: 0;
  color: #6b7280;
  font-size: 17px;
}

.movies-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 25px;
}

.no-results {
  padding: 40px;
  border-radius: 12px;
  background: #ffffff;
  color: #6b7280;
  text-align: center;
  border: 1px solid #e5e7eb;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
  margin-top: 35px;
}

.pagination-button {
  min-width: 42px;
  padding: 10px 14px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background: #ffffff;
  color: #374151;
  font-weight: 600;
  transition: all 0.2s ease;
}

.pagination-button:hover:not(:disabled) {
  background: #f3f4f6;
}

.pagination-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.pagination-button.active {
  border-color: #2563eb;
  background: #2563eb;
  color: #ffffff;
}

.page-number {
  min-width: 40px;
}
</style>