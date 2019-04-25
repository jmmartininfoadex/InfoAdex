import requests

def executeQuery(query, token):
    return requests.post(_url('Query/execute'), json=query, headers={'Authorization': token})

def getAuthToken(apikey, username, password):
    return requests.post(_url('Users/getAuthToken'), json={
	        'ApiKey': apikey,
	        'UserName': username,
	        'PassWord': password
        })

def _url(path):
    return 'http://localhost:57150/v1_0/' + path

