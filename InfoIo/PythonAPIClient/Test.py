import APIClient

def _getQuery():
    return {'Name': 'Test API',
        'Options': {'Consolidated': False,
            'Date_From': '2020-01-01',
            'Date_To': '2020-01-10',
            'Measures': {'Inserciones': False,
                'InvEstudioInfoadex': False,
                'InvTarifa': True,
                'Ocupacion': False
            }
        },
        'Columns': [
            {'Group': 'Otras variables comunes',
                'VariableName': 'Mes'
            }
        ],
        'Rows': [
            {'Group': 'Grupo Medios',
                'VariableName': 'Soporte'
            },
            {'Group': 'Grupo Marcas',
                'VariableName': 'Marca'
            },
            {'Group': 'Grupo Marcas',
                'VariableName': 'Modelo'
            },
            {'Group': 'Grupo Marcas',
                'VariableName': 'Anunciante'
            }
        ],
        'Filter': [
            {'FilterValues': [
                    'DIARIOS'
                ],
                'Group': 'Grupo Medios',
                'VariableName': 'Medio'
            }
        ]
    }
    return ret

resp = APIClient.getAuthToken("apikey", "username", "base64encodedpassword")

if resp.status_code != 200:
    raise Exception('Token error: {}'.format(resp.status_code))

token = resp.json()["token"]

print(resp.json())

resp = APIClient.getQuery({ "QueryType": "CONSULTA", "QueryName": "MasPlus Electrolux"}, token)

if resp.status_code != 200:
    raise Exception('Token error: {}'.format(resp.status_code))

print(resp.json())

resp = APIClient.executeQuery(_getQuery(), token)

if resp.status_code != 200:
    raise Exception('Token error: {}'.format(resp.status_code))

print(resp.json())

creativestoken = resp.json()["creatives_Token"]

resp = APIClient.getCreatives(creativestoken, "SUPLEM. Y DOMINICALES", "3212664")

if resp.status_code != 200:
    raise Exception('Token error: {}'.format(resp.status_code))

print(resp)