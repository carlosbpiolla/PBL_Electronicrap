
// Define um componente React para exibir as informações do endereço selecionado
function AddressInfoWindow({ selectedData, onClose }) {
    return selectedData ? React.createElement(
        'div',
        { className: 'info-window' },
        React.createElement('h3', null, 'Informações do Endereço'),
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
        React.createElement('button', { onClick: onClose }, 'Fechar')
    ) : null;
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

    // Função para buscar a localização pelo endereço completo
    function geocodeAddress(address) {
        return new Promise((resolve, reject) => {
            if (geocoder) {
                geocoder.geocode({ address: address }, function (results, status) {
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

    // Função para extrair dados da tabela HTML e atualizar cepList
    function extractCepDataFromTable() {
        const tableRows = document.querySelectorAll("table.table tbody tr");
        const data = Array.from(tableRows).map(row => {
            const cells = row.querySelectorAll("td");
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

    // Função para adicionar marcadores para todos os endereços na lista
    function handleSearchAddressList() {
        // Limpar marcadores antigos
        markers.forEach(marker => marker.setMap(null));
        setMarkers([]);

        // Geocodificar cada endereço e adicionar um marcador no mapa
        cepList.forEach((item, index) => {
            const address = `${item.address_street} ${item.address_number}, ${item.address_city}, ${item.address_state}, ${item.postal_code}`;
            geocodeAddress(address).then(location => {
                const newMarker = new google.maps.Marker({
                    map: map,
                    position: location,
                    icon: {
                        path: google.maps.SymbolPath.CIRCLE,
                        scale: 10,
                        fillColor: '#0000CD',
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
        React.createElement('button', { onClick: handleSearchAddressList }, 'Buscar Endereços'),
        React.createElement(AddressInfoWindow, { selectedData: selectedData, onClose: () => setSelectedData(null) })
    );
}

// Renderiza o componente Content no elemento com o id "root"
ReactDOM.render(React.createElement(Content, null), document.getElementById('root'));