import { reactive, ref } from 'vue'
import type { Movie } from '../types/movie'
import * as movieService from '../services/movieService'

const movies = reactive<Movie[]>([])
const isLoading = ref(false)
const error = ref<string | null>(null)

export function useMoviesStore() {
  const getMovieById = (id: number) => {
    return movies.find((movie) => movie.id === id)
  }

  const loadMovies = async () => {
    isLoading.value = true
    error.value = null

    try {
      const loadedMovies = await movieService.getMovies()
      movies.splice(0, movies.length, ...loadedMovies)
    } catch (loadError) {
      error.value = 'No fue posible cargar las películas.'
      throw loadError
    } finally {
      isLoading.value = false
    }
  }

  const loadMovie = async (id: number) => {
    const loadedMovie = await movieService.getMovie(id)
    const index = movies.findIndex((movie) => movie.id === id)

    if (index === -1) movies.push(loadedMovie)
    else movies[index] = loadedMovie

    return loadedMovie
  }

  const addMovie = async (movie: Omit<Movie, 'id'>) => {
    movies.push(await movieService.createMovie(movie))
  }

  const updateMovie = async (updatedMovie: Movie) => {
    const savedMovie = await movieService.updateMovie(updatedMovie.id, updatedMovie)
    const index = movies.findIndex((movie) => movie.id === updatedMovie.id)

    if (index !== -1) movies[index] = savedMovie
  }

  const deleteMovie = async (id: number) => {
    await movieService.deleteMovie(id)
    const index = movies.findIndex((movie) => movie.id === id)

    if (index !== -1) movies.splice(index, 1)
  }

  return {
    movies,
    isLoading,
    error,
    loadMovies,
    loadMovie,
    getMovieById,
    addMovie,
    updateMovie,
    deleteMovie,
  }
}
