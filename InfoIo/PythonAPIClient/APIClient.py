import requests
import urllib
import urllib.parse 

def executeQuery(query, token):
    return requests.post(_url('Query/execute'), json=query, headers={'Authorization': 'Bearer ' + token})

def getAuthToken(apikey, username, password):
    return requests.post(_url('Users/getAuthToken'), json={
	        'ApiKey': apikey,
	        'UserName': username,
	        'PassWord': password
        })

def getCreatives(token, medio, codigo):
    return requests.get(_url('creatives/getFile?access_token=' + token + '&med=' + urllib.parse.urlencode(medio) + '&cod=' + codigo))

def getQuery(query, tokenString):
    return requests.post(_url('Query/getQuery'), json=query,  headers={'Authorization': 'Bearer ' + urllib.parse.quote(tokenString, safe='')})

def _url(path):
    return 'https://infoio.infoadex.es/infoioapi/v1.2/' + path

