

#
# Miguel Arribas y Juan Pedro Moreno, de Havasmedia, 2021.
#

library('httr')
library('tidyverse')
library('jsonlite')

infoio_url <- 'https://infoio.infoadex.es/infoioapi/v1.1'


# Completar con la credenciales de acceso.

.credenciales <- list(
  user = '',
  pwd = jsonlite::base64_enc(''),
  api_key = '')

extraer_content <- function(response, json = TRUE) {
  # En windows R no usa utf8 para codificar caracteres. Usar esta línea:
  tmp <- iconv(rawToChar(response$content), from = 'utf8', to = 'cp1252')
  # En Linux / OSX no es necesario convertir, usar esta línea en lugar de la de arriba:
  # tmp <- rawToChar(response$content)
  if (json == TRUE) {
    fromJSON(tmp)
  } else {
    tmp
  }
}


obtener_token <- function(credenciales) {
  response <- POST(url = paste0(infoio_url, '/Users/getAuthToken'), encode = 'json',
                   body = list(ApiKey = credenciales$api_key, UserName = credenciales$user, PassWord = credenciales$pwd))
  if (response$status != 200) {
    stop(response$status)
  }
  tmp <- extraer_content(response)
  if (is.null(tmp$token)) {
    stop(paste0('Error al solicitar token:\n ', tmp$exception$source, '\n ', tmp$exception$message, '\n.'))
  }
  tmp
}


obtener_variables <- function(token) {
  response <- POST(url = paste0(infoio_url, '/Query/getVariables'), encode = 'json',
                   add_headers(Authorization = paste0('Bearer ', token$token)),
                   body = '')
  if (response$status != 200) {
    stop(response$status)
  }
  # La respuesta incluye una columna exception, pero no está claro su uso. La eliminamos.
  extraer_content(response)[, -3]
}


obtener_valores_filtrados <- function(token, grupo, variable, filtro) {
  response <- POST(url = paste0(infoio_url, '/Query/getFilterValues'), encode = 'json',
                   add_headers(Authorization = paste0('Bearer ', token$token)),
                   body = list(FilterValue = filtro, Group = grupo, variableName = variable))
  if (response$status != 200) {
    stop(response$status)
  }
  tmp <- extraer_content(response)
  if (!is.null(tmp$exception)) {
    stop(paste0('Error al solicitar valores filtrados:\n ', tmp$exception$source, '\n ', tmp$exception$message, '\n.'))
  }
  tmp$filterValues
}


obtener_query <- function(token, tipo, nombre) {
  response <- POST(url = paste0(infoio_url, '/Query/getQuery'), encode = 'json',
                   add_headers(Authorization = paste0('Bearer ', token$token)),
                   body = list(QueryType = tipo, QueryName = nombre))
  if (response$status != 200) {
    stop(response$status)
  }
  # extraer_content(response)
  iconv(rawToChar(response$content), from = 'utf8', to = 'cp1252')
}


ejecutar_query <- function(token, estructura) {
  estructura <-
  response <- POST(url = paste0(infoio_url, '/Query/execute'), encode = 'raw',
                   add_headers(Authorization = paste0('Bearer ', token$token), 'Content-Type' = 'application/json'),
                   body = toJSON(estructura, auto_unbox = TRUE, null = 'null'))
  if (response$status != 200) {
    stop(response$status)
  }
  tmp <- extraer_content(response)
  datos <- tmp$data
  colnames(datos) <- tmp$columns$columnHeader
  datos
}


obtener_ponderaciones <- function(token) {
  response <- POST(url = paste0(infoio_url, '/Query/getPonderations'), encode = 'json',
                   add_headers(Authorization = paste0('Bearer ', token$token)),
                   body = '')
  if (response$status != 200) {
    stop(response$status)
  }
  # La respuesta incluye una columna exception, pero no está claro su uso. La eliminamos.
  extraer_content(response)[, -4]
}


# Ejemplo de la definición de una consulta.

definicion_consulta <- list(name = 'Para_API_01',
                            options = list(
                              consolidated = TRUE,
                              date_From = "2020-01-01",
                              date_To = "2020-12-31",
                              measures = list(
                                inserciones = FALSE,
                                invEstudioInfoadex = TRUE,
                                invTarifa = TRUE,
                                ocupacion = FALSE
                              ),
                              ponderation = list(
                                id = 2,
                                description = "Ponderación mensual 2020-2021 (26.04.2021) trimestral 2007-2019",
                                showAllValues = FALSE
                              ),
                              periods = NULL
                            ),
                            columns = list(),
                            rows = list(
                              list(
                                group = "Grupo Marcas",
                                variableName = "Marca"
                              ),
                              list(
                                group = "Otras variables comunes",
                                variableName = "Año"
                              ),
                              list(
                                group = "Otras variables comunes",
                                variableName = "Mes"
                              )
                            ),
                            filter = list(
                              list(
                                filterValues = list("ALIMENTACION"),
                                group = "Grupo Marcas",
                                variableName = "Sector"
                              ),
                              list(
                                filterValues = list(
                                  "[YOGURES Y POSTRES FRESCOS][ALIMENTACION]",
                                  "[YOGURES/POSTRES FRES.DES.][ALIMENTACION]"
                                ),
                                group = "Grupo Marcas",
                                variableName = "Categoría"
                              )
                            ))

token <- obtener_token(.credenciales)

datos <- ejecutar_query(token, definicion_consulta)




