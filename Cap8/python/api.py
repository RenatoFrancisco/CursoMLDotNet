import requests

print('Integracao do Python com API ASP.NET Core')

url = 'https://localhost:5001/api/bikedemanda'
params = {
    'Season' : 4,
	'Year' : 1,
	'Month' : 11,
	'Hour' : 23,
	'Holiday' : 0,
	'Weekday' : 5,
	'WorkingDay' : 1,
	'Weather' : 1,
	'Temperature' : 0.32,
	'NormalizedTemperature' : 0.3333,
	'Humidity' : 0.81,
	'Windspeed' : 0.089
}

r = requests.get(url, params=params, verify=False)
print('Status de retorno:', r.status_code)
print('Retorno da API', r.text)
