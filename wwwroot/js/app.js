// Define um componente React para exibir a tabela de CEPs
function CepTable({ data }) {
    return React.createElement(
        'table',
        null,
        React.createElement(
            'thead',
            null,
            React.createElement(
                'tr',
                null,
                React.createElement('th', null, 'Nome'),
                React.createElement('th', null, 'Idade'),
                React.createElement('th', null, 'CEP')
            )
        ),
        React.createElement(
            'tbody',
            null,
            data.map((item, index) =>
                React.createElement(
                    'tr',
                    { key: index },
                    React.createElement('td', null, item.nome),
                    React.createElement('td', null, item.idade),
                    React.createElement('td', null, item.cep)
                )
            )
        )
    );
}

// Define um componente React para o conteúdo principal
function Content() {
    const [map, setMap] = React.useState(null);
    const [geocoder, setGeocoder] = React.useState(null);
    const [cepList, setCepList] = React.useState([
        { nome: 'Alice', idade: 25, cep: '01001-000' },
        { nome: 'Bob', idade: 30, cep: '02002-000' },
        { nome: 'Charlie', idade: 35, cep: '03003-000' }
    ]); // Lista de dados com nome, idade e CEP
    const [markers, setMarkers] = React.useState([]);
    const [selectedData, setSelectedData] = React.useState(null);

    // Função para inicializar o mapa
    function initMap() {
        // Coordenadas do centro do mapa
        var center = { lat: -23.5505, lng: -46.6333 };

        // Opções do mapa
        var mapOptions = {
            zoom: 10,
            center: center
        };

        // Criação do mapa
        var mapInstance = new google.maps.Map(document.getElementById('map'), mapOptions);
        setMap(mapInstance);

        // Inicializar o geocoder
        var geocoderInstance = new google.maps.Geocoder();
        setGeocoder(geocoderInstance);

        // Obter a localização do usuário
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var userLatLng = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };

                // Adicionar marcador para a localização do usuário
                var userMarker = new google.maps.Marker({
                    position: userLatLng,
                    map: mapInstance,
                    title: 'Sua localização'
                });

                // Centralizar o mapa na localização do usuário
                mapInstance.setCenter(userLatLng);
            }, function () {
                console.error('Erro ao obter a localização do usuário.');
            });
        } else {
            console.error('Navegador não suporta geolocalização.');
        }
    }

    // Função para buscar a localização pelo CEP
    function geocodeCep(cep) {
        return new Promise((resolve, reject) => {
            if (geocoder) {
                geocoder.geocode({ address: cep }, function (results, status) {
                    if (status === 'OK') {
                        resolve(results[0].geometry.location);
                    } else {
                        reject('Geocode não foi bem sucedido por causa de: ' + status);
                    }
                });
            } else {
                reject('Geocoder não está inicializado.');
            }
        });
    }

    // Função para adicionar marcadores para todos os CEPs na lista
    function handleSearchCepList() {
        // Limpar marcadores antigos
        markers.forEach(marker => marker.setMap(null));
        setMarkers([]);

        // Geocodificar cada CEP e adicionar um marcador no mapa
        cepList.forEach((item, index) => {
            geocodeCep(item.cep).then(location => {
                const newMarker = new google.maps.Marker({
                    map: map,
                    position: location,
                    icon: {
                        path: google.maps.SymbolPath.CIRCLE,
                        scale: 10,
                        fillColor: '#228B22',
                        fillOpacity: 1,
                        strokeWeight: 1
                    }
                });

                // Adicionar evento de clique no marcador
                newMarker.addListener('click', () => {
                    setSelectedData(item);
                });

                setMarkers(prevMarkers => [...prevMarkers, newMarker]);
            }).catch(error => {
                console.error(error);
            });
        });
    }

    // Chama a função para inicializar o mapa quando o componente for montado
    React.useEffect(() => {
        initMap();
    }, []);

    return React.createElement(
        'div',
        { className: 'content' },
        React.createElement('div', { id: 'map', style: { height: '400px' } }),
        React.createElement('button', { onClick: handleSearchCepList }, 'Buscar CEPs'),
        React.createElement(CepTable, { data: cepList }),
        selectedData && React.createElement(
            'div',
            { className: 'info-window' },
            React.createElement('h3', null, 'Informações do CEP'),
            React.createElement('p', null, `Nome: ${selectedData.nome}`),
            React.createElement('p', null, `Idade: ${selectedData.idade}`),
            React.createElement('p', null, `CEP: ${selectedData.cep}`),
            React.createElement('button', { onClick: () => setSelectedData(null) }, 'Fechar')
        )
    );
}

// Renderiza o componente Menu e Content no elemento com o id "root"
ReactDOM.render(React.createElement(
    'div',
    null,
    React.createElement(Content, null)
), document.getElementById('root'));