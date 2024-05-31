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
                React.createElement('th', null, 'Nome Fantasia'),
                React.createElement('th', null, 'Telefone'),
                React.createElement('th', null, 'CEP'),
                React.createElement('th', null, 'Rua'),
                React.createElement('th', null, 'Número'),
                React.createElement('th', null, 'Complemento'),
                React.createElement('th', null, 'Bairro'),
                React.createElement('th', null, 'Cidade'),
                React.createElement('th', null, 'Estado'),
                React.createElement('th', null, 'Email'),
                React.createElement('th', null, 'Tipo Lixo Coletado')
            )
        ),
        React.createElement(
            'tbody',
            null,
            data.map((item, index) =>
                React.createElement(
                    'tr',
                    { key: index },
                    React.createElement('td', null, item.fantasy_name),
                    React.createElement('td', null, item.phone_number),
                    React.createElement('td', null, item.postal_code),
                    React.createElement('td', null, item.address_street),
                    React.createElement('td', null, item.address_number),
                    React.createElement('td', null, item.address_complement),
                    React.createElement('td', null, item.address_district),
                    React.createElement('td', null, item.address_city),
                    React.createElement('td', null, item.address_state),
                    React.createElement('td', null, item.email),
                    React.createElement('td', null, item.categoriaDescricao)
                )
            )
        )
    );
}

// Define um componente React para o conteúdo principal
function Content() {
    const [map, setMap] = React.useState(null);
    const [geocoder, setGeocoder] = React.useState(null);
    const [cepList, setCepList] = React.useState([]);
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
            geocodeCep(item.postal_code).then(location => {
                const newMarker = new google.maps.Marker({
                    map: map,
                    position: location,
                    icon: {
                        path: google.maps.SymbolPath.CIRCLE,
                        scale: 10,
                        fillColor: '#FF0000',
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

    // Função para extrair dados da tabela HTML e atualizar o estado do React
    function extractCepDataFromTable() {
        const table = document.getElementById('cepTable');
        const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        const data = Array.from(rows).map(row => {
            const cells = row.getElementsByTagName('td');
            return {
                fantasy_name: cells[0].innerText,
                phone_number: cells[1].innerText,
                postal_code: cells[2].innerText,
                address_street: cells[3].innerText,
                address_number: cells[4].innerText,
                address_complement: cells[5].innerText,
                address_district: cells[6].innerText,
                address_city: cells[7].innerText,
                address_state: cells[8].innerText,
                email: cells[9].innerText,
                categoriaDescricao: cells[10].innerText
            };
        });
        setCepList(data);
    }

    // Chama a função para inicializar o mapa e extrair dados da tabela quando o componente for montado
    React.useEffect(() => {
        // Verifica se a API do Google Maps foi carregada
        if (typeof google !== 'undefined' && google.maps) {
            initMap();
            extractCepDataFromTable();
        } else {
            console.error('Google Maps API não foi carregada.');
        }
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
            React.createElement('p', null, `Nome Fantasia: ${selectedData.fantasy_name}`),
            React.createElement('p', null, `Telefone: ${selectedData.phone_number}`),
            React.createElement('p', null, `CEP: ${selectedData.postal_code}`),
            React.createElement('p', null, `Rua: ${selectedData.address_street}`),
            React.createElement('p', null, `Número: ${selectedData.address_number}`),
            React.createElement('p', null, `Complemento: ${selectedData.address_complement}`),
            React.createElement('p', null, `Bairro: ${selectedData.address_district}`),
            React.createElement('p', null, `Cidade: ${selectedData.address_city}`),
            React.createElement('p', null, `Estado: ${selectedData.address_state}`),
            React.createElement('p', null, `Email: ${selectedData.email}`),
            React.createElement('p', null, `Tipo Lixo Coletado: ${selectedData.categoriaDescricao}`),
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
